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
    public class CountAllAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sql = GenericSql(context, next);

            var paramters = context.GetParameters();
            var queryParams = ToParamterDict(paramters);
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var result = QueryHelper.RunGenericCount(context.ServiceMethod.ReturnType, conn, sql, queryParams);
                context.ReturnValue = result;
            }

            return context.Break();
        }

        private string GenericSql(AspectContext context, AspectDelegate next)
        {
            var builder = SqlBuilderSelecter.Get(Provider);

            var gt = AgileRepositoryGenericTypeArguments(context);
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "Count", gt, builder,
                new object[] {});

            return sql;
        }

    }
}
