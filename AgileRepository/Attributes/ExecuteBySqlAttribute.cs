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
    public class ExecuteBySqlAttribute : SqlAttribute
    {
        public string Sql { get; set; }

        public ExecuteBySqlAttribute(string sql)
        {
            Sql = sql;
        }

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (string.IsNullOrEmpty(Sql))
            {
                throw new ArgumentNullException(nameof(Sql));
            }

            var paramters = context.GetParameters();
            var sqlParamters = ToParamterDict(paramters);
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                //get IAgileRepository<TEntity> 's TEntity for Query T
                var result = (int)QueryHelper.RunExecute(conn, Sql, sqlParamters);
                if (context.ServiceMethod.ReturnType == typeof(bool))
                {
                    context.ReturnValue = result >= 0;
                }
                else
                {
                    context.ReturnValue = result;
                }

            }

            return context.Break();
        }

    }
}
