using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
using Agile.Repository.Sql;
using Agile.Repository.Utils;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;

namespace Agile.Repository.Attributes
{
    public class QueryBySqlAttribute : AbstractInterceptorAttribute
    {
        public string Sql { get; set; }

        public string ConnectionName { get; set; }

        public QueryBySqlAttribute(string sql)
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
            var queryParams = new Dictionary<string, object>();
            foreach (var parameter in paramters)
            {
                queryParams.Add(parameter.Name, parameter.Value);
            }
            using (var conn = ConnectionFactory.CreateConnection(ConnectionName))
            {
                var result = QueryHelper.RunGenericQuery(context, conn, Sql, queryParams);
                context.ReturnValue = result;
            }

            return context.Break();
        }

    }
}
