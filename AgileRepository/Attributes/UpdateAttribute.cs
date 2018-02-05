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
    public class UpdateAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var paramters = context.GetParameters();
         
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var sql = GenericUpdateSql(context);
                var result = (int)QueryHelper.RunExecute(conn, sql, paramters.First().Value);
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

        private string GenericUpdateSql(AspectContext context)
        {
            var provider = string.IsNullOrEmpty(ConnectionName)
                ? DbProviders.Sqlserver
                : ConnectionConfig.GetProviderName(ConnectionName);
            var builder = SqlBuilderSelecter.Get(provider);
            var gt = AgileRepositoryGenericTypeArguments(context);
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "UpdateById", gt, builder,
                new object[] {});

            return sql;
        }

    }
}
