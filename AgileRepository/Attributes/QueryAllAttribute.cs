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
    public class QueryAllAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sql = GenericSql(context, next);

            var paramters = context.GetParameters();
            var queryParams = ToParamterDict(paramters);
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var gt = AgileRepositoryGenericTypeArguments(context);
                var result = QueryHelper.RunGenericQuery(gt.First(), conn, sql, queryParams);
                context.ReturnValue = result;
            }

            return context.Break();
        }

        private string GenericSql(AspectContext context, AspectDelegate next)
        {
            var builder = SqlBuilderSelecter.Get(Provider);

            var gt = AgileRepositoryGenericTypeArguments(context);
            //get IAgileRepository<TEntity> 's TEntity for MethodNameToSql's T
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "Select", gt, builder,
                new object[] {});

            return sql;
        }

    }
}
