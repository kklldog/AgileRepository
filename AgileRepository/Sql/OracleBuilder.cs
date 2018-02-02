using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace Agile.Repository.Sql
{
    public class OracleBuilder : SqlBuilder
    {
        private ISqlGenerator _sqlGenerator;

        public override string QueryParamSyntaxMark => ":";

        public override ISqlGenerator SqlGenerator
        {
            get
            {
                if (_sqlGenerator == null)
                {
                    var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new OracleDialect());
                    _sqlGenerator = new SqlGeneratorImpl(config);
                }

                return _sqlGenerator;
            }
        }


    }
}

