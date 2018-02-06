using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
using Agile.Repository.Utils;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace Agile.Repository.Sql
{
    public interface ISqlBuilder
    {
        string QueryParamSyntaxMark { get; }
        ISqlGenerator SqlGenerator { get; }
        string Select<T>() where T : class;
        string Count<T>() where T : class;
        string Insert<T>() where T : class;
        string Update<T>() where T : class;
        string UpdateById<T>() where T : class;
        string Delete<T>() where T : class;
        string DeleteById<T>() where T : class;
        string MethodNameToWhere(string methodName);
        string MethodNameToSql<T>(string methodName) where T : class;
    }

    public class SqlInnerKey
    {
        public string Name { get; set; }

        public string SqlString { get; set; }

    }

    public class MethodPrefixsKey
    {
        public string Name { get; set; }

        public string GeneratorMethod { get; set; }

        public bool NeedWhere { get; set; }

    }

    public abstract class SqlBuilder : ISqlBuilder
    {
        protected readonly List<MethodPrefixsKey> MethodPrefixs;
        protected readonly List<string> OpKeys;
        protected readonly List<SqlInnerKey> InnerKeys;

        protected readonly ConcurrentDictionary<string, string> SqlCacheDict;

        protected SqlBuilder()
        {
            SqlCacheDict = new ConcurrentDictionary<string, string>();

            MethodPrefixs = new List<MethodPrefixsKey>()
            {
               new MethodPrefixsKey()
               {
                   Name =  "QueryBy",
                   GeneratorMethod = "Select",
                   NeedWhere = true
               },
                new MethodPrefixsKey()
                {
                    Name =  "CountBy",
                    GeneratorMethod = "Count",
                    NeedWhere = true
                }
                ,
                new MethodPrefixsKey()
                {
                    Name =  "DeleteBy",
                    GeneratorMethod = "Delete",
                    NeedWhere = true
                }
            };

            OpKeys = new List<string>()
            {
                "And","Or"
            };

            InnerKeys = new List<SqlInnerKey>()
            {
                new SqlInnerKey()
                {
                    Name = "IsNull",
                    SqlString = "Is Null"
                },
                new SqlInnerKey()
                {
                    Name = "IsNotNull",
                    SqlString = "Is Not Null"
                }
            };
        }

        public abstract string QueryParamSyntaxMark { get; }
        public abstract ISqlGenerator SqlGenerator { get; }

        public virtual string Select<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"Select_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var map = SqlGenerator.Configuration.GetMap<T>();
            sql = SqlGenerator.Select(map, null, null, new Dictionary<string, object>());

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string Count<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"Count_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var map = SqlGenerator.Configuration.GetMap<T>();
            sql = SqlGenerator.Count(map, null, new Dictionary<string, object>());

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string Insert<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"Insert_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var map = SqlGenerator.Configuration.GetMap<T>();
            sql = SqlGenerator.Insert(map);

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string Update<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"Update_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var map = SqlGenerator.Configuration.GetMap<T>();
            var columns = map.Properties.Where(p => !(p.Ignored || p.IsReadOnly) && p.KeyType == KeyType.NotAKey);
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
                columns.Select(p => $"{SqlGenerator.GetColumnName(map, p, false)} = {QueryParamSyntaxMark}{p.Name}");
            sql = $"UPDATE {SqlGenerator.GetTableName(map)} SET {setSql.AppendStrings()} ";

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string UpdateById<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"UpdateById_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var update = Update<T>();
            var map = SqlGenerator.Configuration.GetMap<T>();
            var keys = map.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            var whereSql = keys.Select(k => string.Format("{0} = {1}{0}", k.Name, QueryParamSyntaxMark));

            sql = $"{update} WHERE {whereSql.AppendStrings()}";

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string Delete<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"Delete_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var map = SqlGenerator.Configuration.GetMap<T>();
            sql = $"DELETE FROM {SqlGenerator.GetTableName(map)}";

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        public string DeleteById<T>() where T : class
        {
            string sql = "";
            var cacheKey = $"DeleteById_{typeof(T)}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            var delete = Delete<T>();
            var map = SqlGenerator.Configuration.GetMap<T>();
            var keys = map.Properties.Where(p => p.KeyType != KeyType.NotAKey);
            var whereSql = keys.Select(k => string.Format("{0} = {1}{0}", k.Name, QueryParamSyntaxMark));
            sql = $"{delete} WHERE {whereSql.AppendStrings()}";

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

        protected void SplitParams(Queue<string> queryparams, string[] opKeys, string methodName)
        {
            int? startindex = null;
            var opKey = "";
            for (int i = 0; i < opKeys.Length; i++)
            {
                var key = opKeys[i];
                var index = methodName.IndexOf(key, StringComparison.Ordinal);

                if (index >= 0)
                {
                    if (!startindex.HasValue)
                    {
                        startindex = index;
                        opKey = key;
                    }
                    else
                    {
                        if (index < startindex)
                        {
                            startindex = index;
                            opKey = key;
                        }
                    }
                }
            }
            if (startindex.HasValue && startindex >= 0)
            {
                if (startindex > 0)
                {
                    //说明opkey在中间，取前面的参数名
                    var before = methodName.Substring(0, startindex.Value);
                    queryparams.Enqueue(before);
                    //把剩下的内容递归
                    var last = methodName.Substring(startindex.Value);
                    SplitParams(queryparams, opKeys, last);
                }
                if (startindex == 0)
                {
                    //说明opkey是第一个字符
                    queryparams.Enqueue(opKey);
                    var last = methodName.Substring(opKey.Length);
                    SplitParams(queryparams, opKeys, last);
                }
            }
            else
            {
                queryparams.Enqueue(methodName);
            }

        }

        protected string BuildWhere(Queue<string> queryParams)
        {
            var where = new StringBuilder("");
            while (queryParams.Count > 0)
            {
                var param = queryParams.Dequeue();
                if (OpKeys.Contains(param))
                {
                    //is key
                    where.AppendFormat(" {0}", param);
                }
                else
                {
                    SqlInnerKey innerKey = null;
                    foreach (var sqlInnerKey in InnerKeys)
                    {
                        if (param.Contains(sqlInnerKey.Name))
                        {
                            innerKey = sqlInnerKey;
                            break;
                        }
                    }
                    if (innerKey != null)
                    {
                        //username is null ...
                        param = param.Replace(innerKey.Name, "");
                        where.AppendFormat(" {0} {1}", param, innerKey.SqlString);
                    }
                    else
                    {
                        //username=@username
                        where.AppendFormat(" {0}={1}{0}", param, QueryParamSyntaxMark);
                    }

                }
            }

            return where.ToString();
        }

        public virtual string MethodNameToWhere(string methodName)
        {
            //QueryByUserNameAndIdOrNickNameAndChineseName
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            var methodNameCopy = methodName;

            MethodPrefixsKey methodPreix = null;
            foreach (var prefix in MethodPrefixs)
            {
                methodPreix = prefix;
                if (methodNameCopy.StartsWith(prefix.Name))
                {
                    methodNameCopy = methodNameCopy.Replace(prefix.Name, "");
                    break;
                }
            }

            var queryparms = new Queue<string>();
            SplitParams(queryparms, OpKeys.ToArray(), methodNameCopy);
            var where = BuildWhere(queryparms);

            return where;
        }

        public virtual string MethodNameToSql<T>(string methodName) where T : class
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            string sql = "";
            var cacheKey = $"MethodNameToSql_{typeof(T)}_{methodName}";
            var hit = SqlCacheDict.TryGetValue(cacheKey, out sql);
            if (hit)
            {
                return sql;
            }

            MethodPrefixsKey methodPreix = null;
            foreach (var prefix in MethodPrefixs)
            {
                methodPreix = prefix;
                if (methodName.StartsWith(prefix.Name))
                {
                    break;
                }
            }

            if (methodPreix == null)
            {
                throw new Exception(string.Format("Can not hit sql method from method name :{0}", methodName));
            }

            var select = GenericCallHelper.RunGenericMethod(this.GetType(), methodPreix.GeneratorMethod,
                new Type[] { typeof(T) }, this, null);
            var where = "";
            if (methodPreix.NeedWhere)
            {
                where = " where " + MethodNameToWhere(methodName);
            }
            sql = select + where;

            SqlCacheDict.TryAdd(cacheKey, sql);

            return sql;
        }

    }

    public class SqlBuilderSelecter
    {
        private static ConcurrentDictionary<string, ISqlBuilder> _builders = new ConcurrentDictionary<string, ISqlBuilder>();

        public static ISqlBuilder Get(string providerName)
        {
            ISqlBuilder builder = null;
            if (_builders.TryGetValue(providerName, out builder))
            {
                return builder;
            }

            builder = CreateBuilderByProviderName(providerName);
            _builders.TryAdd(providerName, builder);

            return builder;
        }

        private static ISqlBuilder CreateBuilderByProviderName(string providerName)
        {
            if (providerName.Equals(DbProviders.Sqlserver, StringComparison.CurrentCultureIgnoreCase))
            {
                return new SqlserverBuilder();
            }

            if (providerName.Equals(DbProviders.Oracle, StringComparison.CurrentCultureIgnoreCase))
            {
                return new OracleBuilder();
            }

            return new SqlserverBuilder();
        }
    }
}
