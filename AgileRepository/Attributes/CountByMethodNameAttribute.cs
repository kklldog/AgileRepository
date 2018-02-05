using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
using Agile.Repository.Proxy;
using Agile.Repository.Sql;
using Agile.Repository.Utils;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Agile.Repository.Attributes
{
    public class CountByMethodNameAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sql = GenericSqlByMethodName(context, next);

            var paramters = context.GetParameters();
            var queryParams = ToParamterDict(paramters);
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var result = QueryHelper.RunGenericCount(context.ServiceMethod.ReturnType, conn, sql, queryParams);
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

            var gt = AgileRepositoryGenericTypeArguments(context);
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "MethodNameToSql", gt, builder,
                new object[] { context.ProxyMethod.Name });

            return sql;
        }

    }
}
