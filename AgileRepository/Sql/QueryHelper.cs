using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Utils;
using AspectCore.DynamicProxy;
using Dapper;

namespace Agile.Repository.Sql
{
    class QueryHelper
    {
        private static QueryHelper _instance = new QueryHelper();
        private static MethodInfo _queryMethod;
        private static readonly object _lock = new object();

        public static object RunGenericQuery(AspectContext context, IDbConnection conn, string sql, object paramters)
        {
            if (_queryMethod == null)
            {
                lock (_lock)
                {
                    if (_queryMethod == null)
                    {
                        _queryMethod = GenericCallHelper.GetGenericMethod(typeof(QueryHelper), "Query",
                            context.ProxyMethod.ReturnType.GenericTypeArguments);
                    }
                }
            }

            return _queryMethod.Invoke(_instance, new object[]
            {
                conn,sql,paramters
            });
        }

        public object Query<T>(IDbConnection conn, string sql, object paramters) where T : class
        {
            return conn.Query<T>(sql, paramters);
        }
    }
}
