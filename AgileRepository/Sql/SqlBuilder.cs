using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Data;
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
        string MethodNameToWhere(string methodName);
        string MethodNameToSql(string methodName);
    }

    public abstract class SqlBuilder : ISqlBuilder
    {
        protected readonly List<string> MethodPrefixs;
        protected readonly List<string> OpKeys;

        protected SqlBuilder()
        {
            MethodPrefixs = new List<string>()
            {
                "QueryBy"
            };

            OpKeys = new List<string>()
            {
                "And","Or"
            };
        }

        public abstract string QueryParamSyntaxMark { get; }
        public abstract ISqlGenerator SqlGenerator { get; }

        public string Select<T>() where T : class
        {
            var map = SqlGenerator.Configuration.GetMap<T>();
            var sql = SqlGenerator.Select(map, null, null, new Dictionary<string, object>());

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
            var where = new StringBuilder(" where");
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
                    where.AppendFormat(" {0}={1}{0}", param, QueryParamSyntaxMark);
                }
            }

            return where.ToString();
        }

        public string MethodNameToWhere(string methodName)
        {
            //QueryByUserNameAndIdOrNickNameAndChineseName
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            var methodNameCopy = methodName;

            string queryPreix = "";
            foreach (var prefix in MethodPrefixs)
            {
                queryPreix = prefix;
                if (methodNameCopy.StartsWith(queryPreix))
                {
                    methodNameCopy = methodNameCopy.Replace(prefix, "");
                    break;
                }
            }

            var queryparms = new Queue<string>();
            SplitParams(queryparms, OpKeys.ToArray(), methodNameCopy);
            var where = BuildWhere(queryparms);

            return where;
        }

        public abstract string MethodNameToSql(string methodName);

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
            if (providerName.Equals(DbProviders.Sqlserver,StringComparison.CurrentCultureIgnoreCase))
            {
                return new SqlserverBuilder();
            }

            if (providerName.Equals(DbProviders.Oracle, StringComparison.CurrentCultureIgnoreCase))
            {
                return new SqlserverBuilder();
            }

            return new SqlserverBuilder();
        }
    }
}
