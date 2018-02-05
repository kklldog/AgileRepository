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
    public class DeleteByMethodNameAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var paramters = context.GetParameters();
            var queryParams = ToParamterDict(paramters);
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var sql = GenericDeleteSql(context);
                var result = (int)QueryHelper.RunExecute(conn, sql, queryParams);
                if (context.ServiceMethod.ReturnType == typeof(bool))
                {
                    context.ReturnValue = result > 0;
                }
                else
                {
                    context.ReturnValue = result;
                }
            }

            return context.Break();
        }

        private string GenericDeleteSql(AspectContext context)
        {
            var provider = string.IsNullOrEmpty(ConnectionName)
                ? DbProviders.Sqlserver
                : ConnectionConfig.GetProviderName(ConnectionName);
            var builder = SqlBuilderSelecter.Get(provider);
            var gt = context.ServiceMethod.DeclaringType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAgileRepository<>))
                .GenericTypeArguments;

            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "MethodNameToSql", gt, builder,
                new object[] { context.ProxyMethod.Name });

            return sql;
        }

    }
}
