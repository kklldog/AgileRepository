using System;
using System.Collections;
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
                var updateParam = paramters.First().Value;
                int result = 0;
                if ((updateParam as IEnumerable) == null)
                {
                    result = (int)QueryHelper.RunExecute(conn, sql, paramters.First().Value);
                }
                else
                {
                    using (var tran = conn.BeginTransaction())
                    {
                        foreach (var p in (updateParam as IEnumerable))
                        {
                            result =+ (int)QueryHelper.RunExecute(conn,tran, sql, p);
                        }

                        tran.Commit();
                    }
                }

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
            var builder = SqlBuilderSelecter.Get(Provider);
            var gt = AgileRepositoryGenericTypeArguments(context);
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "UpdateById", gt, builder,
                new object[] { });

            return sql;
        }

    }
}
