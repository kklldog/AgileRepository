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
    public class QueryByNameAttribute : AbstractInterceptorAttribute
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
                var result = QueryHelper.RunGenericQuery(context.ServiceMethod.ReturnType, conn, sql, queryParams);
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

            var gt = context.ServiceMethod.DeclaringType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAgileRepository<>))
                .GenericTypeArguments;
            //get IAgileRepository<TEntity> 's TEntity for MethodNameToSql's T
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "MethodNameToSql", gt, builder,
                new object[] { context.ProxyMethod.Name });

            return sql;
        }

    }
}
