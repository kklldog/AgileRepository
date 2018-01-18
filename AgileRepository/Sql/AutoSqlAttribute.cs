using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
using Agile.Repository.Utils;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Agile.Repository.Sql
{
    public class AutoSqlAttribute : AbstractInterceptorAttribute
    {
        public string Sql { get; set; }

        public string ConnectionName { get; set; }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (!string.IsNullOrEmpty(Sql))
            {
                //run sql direct 
                var paramters = context.GetParameters();
                var queryParams = new Dictionary<string, object>();
                foreach (var parameter in paramters)
                {
                    queryParams.Add(parameter.Name, parameter.Value);
                }
                using (var conn = ConnectionFactory.CreateConnection())
                {
                    var reuslt = QueryHelper.RunGenericQuery(context, conn, Sql, queryParams);
                    context.ReturnValue = reuslt;
                }
            }
            else
            {
                ByMethodName(context,next);
            }

            return context.Break();
        }

        private void ByMethodName(AspectContext context, AspectDelegate next)
        {
            var paramters = context.GetParameters();
            var queryParams = new Dictionary<string, object>();
            foreach (var parameter in paramters)
            {
                queryParams.Add(parameter.Name, parameter.Value);
            }

            var provider = string.IsNullOrEmpty(ConnectionName)
                ? DbProviders.Sqlserver
                : ConnectionConfig.GetProviderName(ConnectionName);
            var builder = SqlBuilderSelecter.Get(provider);

            var select = GenericCallHelper.RunGenericMethod(builder.GetType(), "Select",
                context.ProxyMethod.ReturnType.GenericTypeArguments, builder, new object[] { });
            var where = builder.MethodNameToWhere(context.ProxyMethod.Name);

            var sql = (string) select + where;
            using (var conn = ConnectionFactory.CreateConnection())
            {
                var reuslt = QueryHelper.RunGenericQuery(context, conn, sql, queryParams);
                context.ReturnValue = reuslt;
            }

        }

    }
}
