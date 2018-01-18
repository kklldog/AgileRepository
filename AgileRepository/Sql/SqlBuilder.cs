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
        string Select<T>() where T : class;
        string MethodNameToWhere(string methodName);
    }

    public class SqlBuilder : ISqlBuilder
    {
        private readonly ISqlGenerator _sqlGenerator;

        public SqlBuilder(string providerName)
        {
            var dialect = ConnectionFactory.GetSqlDialect(providerName ?? DbProviders.Sqlserver);
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), dialect);

            _sqlGenerator = new SqlGeneratorImpl(config);
        }

        public string Select<T>() where T : class
        {
            var map = _sqlGenerator.Configuration.GetMap<T>();
            var sql = _sqlGenerator.Select(map, null, null, new Dictionary<string, object>());

            return sql;
        }

        public string MethodNameToWhere(string methodName)
        {
            const string queryPreix = "QueryBy";
            methodName = methodName.Replace(queryPreix, "");

            string key = "And";

            var arr = methodName.Split(new string[] {key}, StringSplitOptions.None);

            var where = new StringBuilder(" where 1=1 ");
            foreach (var s in arr)
            {
                where.AppendFormat("{1} {0}=@{0} ", s, key);
            }

            return where.ToString();
        }

    }

    class SqlBuilderSelecter
    {
        private static ConcurrentDictionary<string, ISqlBuilder> _builders = new ConcurrentDictionary<string, ISqlBuilder>();

        public static ISqlBuilder Get(string providerName)
        {
            ISqlBuilder builder = null;
            if (_builders.TryGetValue(providerName, out builder))
            {
                return builder;
            }

            builder = new SqlBuilder(providerName);
            _builders.TryAdd(providerName, builder);

            return builder;
        }
    }
}
