using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
using Agile.Repository.Sql;
using Agile.Repository.Utils;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Agile.Repository.Attributes
{
    public class CountByNameAttribute : AbstractInterceptorAttribute
    {
        public string ConnectionName { get; set; }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sql = GenericSqlByMethodName(context, next);

            var paramters = context.GetParameters();
            var queryParams = new Dictionary<string, object>();
            foreach (var parameter in paramters)
            {
                queryParams.Add(parameter.Name, parameter.Value);
            }
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var result = QueryHelper.RunGenericCount(context, conn, sql, queryParams);
                context.ReturnValue = result;
            }

            return context.Break();
        }

        private string GenericSqlByMethodName(AspectContext context, AspectDelegate next)
        {
            var provider = string.IsNullOrEmpty(ConnectionName)
                ? DbProviders.Sqlserver
                : ConnectionConfig.GetProviderName(ConnectionName);
            var builder = SqlBuilderSelecter.Get(provider);

            var gt = context.ProxyMethod.GetGenericArguments();
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "MethodNameToSql", gt, builder,
                new object[] { context.ProxyMethod.Name });

            return sql;
        }

    }
}
