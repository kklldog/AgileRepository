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
    public class InsertAttribute : SqlAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var paramters = context.GetParameters();

            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var sql = GenericInsertSql(context);
                var insertParam = paramters.First();
                var isIEnumerable = (insertParam as IEnumerable) != null;
                var result = 0;
                if (!isIEnumerable)
                {
                    result = (int)QueryHelper.RunExecute(conn, sql, paramters.First().Value);
                }
                else
                {
                    //for insert entities
                    using (var tran = conn.BeginTransaction())
                    {
                        foreach (var p in (IEnumerable)insertParam)
                        {
                            result += (int)QueryHelper.RunExecute(conn, tran, sql, p);
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

        private string GenericInsertSql(AspectContext context)
        {
            var builder = SqlBuilderSelecter.Get(Provider);
            var gt = AgileRepositoryGenericTypeArguments(context);
            var sql = (string)GenericCallHelper.RunGenericMethod(builder.GetType(), "Insert", gt, builder,
                new object[] { });

            return sql;
        }

    }
}
