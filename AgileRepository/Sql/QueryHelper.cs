using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Agile.Repository.Utils;
using Dapper;

namespace Agile.Repository.Sql
{
    class QueryHelper
    {
        private static QueryHelper _instance = new QueryHelper();
        private static MethodInfo _queryMethod;
        private static MethodInfo _countMethod;
        private static readonly object _lock = new object();

        public static object RunGenericQuery(Type returnType, IDbConnection conn, string sql, object paramters)
        {
            if (_queryMethod == null)
            {
                lock (_lock)
                {
                    if (_queryMethod == null)
                    {
                        _queryMethod = GenericCallHelper.GetGenericMethod(typeof(QueryHelper), "Query",
                            new Type[] { returnType });
                    }
                }
            }

            return _queryMethod.Invoke(_instance, new object[]
            {
                conn,sql,paramters
            });
        }

        public static object RunGenericCount(Type returnType, IDbConnection conn, string sql, object paramters)
        {
            if (_countMethod == null)
            {
                lock (_lock)
                {
                    if (_countMethod == null)
                    {
                        _countMethod = GenericCallHelper.GetGenericMethod(typeof(QueryHelper), "Count",
                            new Type[] { returnType });
                    }
                }
            }

            return _countMethod.Invoke(_instance, new object[]
            {
                conn,sql,paramters
            });
        }

        public static object RunExecute(IDbConnection conn, string sql, object paramters)
        {
            if (AgileRepository.Config != null && AgileRepository.Config.SqlMonitor != null)
            {
                AgileRepository.Config.SqlMonitor(sql, paramters);
            }

            return conn.Execute(sql, paramters);
        }

        public static object RunExecute(IDbConnection conn, IDbTransaction tran, string sql, object paramters)
        {
            if (AgileRepository.Config != null && AgileRepository.Config.SqlMonitor != null)
            {
                AgileRepository.Config.SqlMonitor(sql, paramters);
            }

            return conn.Execute(sql, paramters, tran);
        }


        public object Query<T>(IDbConnection conn, string sql, object paramters) where T : class
        {
            if (AgileRepository.Config != null && AgileRepository.Config.SqlMonitor != null)
            {
                AgileRepository.Config.SqlMonitor(sql, paramters);
            }

            return conn.Query<T>(sql, paramters);
        }

        public object Count<T>(IDbConnection conn, string sql, object paramters)
        {
            if (AgileRepository.Config != null && AgileRepository.Config.SqlMonitor != null)
            {
                AgileRepository.Config.SqlMonitor(sql, paramters);
            }

            return conn.ExecuteScalar<T>(sql, paramters);
        }

    }
}
