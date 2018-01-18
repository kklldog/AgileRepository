using Microsoft.VisualStudio.TestTools.UnitTesting;
using Agile.Repository.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileRepository.Proxy.Tests;

namespace Agile.Repository.Sql.Tests
{
    [TestClass()]
    public class SqlBuilderTests
    {
        [TestMethod()]
        public void SelectTest()
        {
            var builder = new SqlBuilder(null);

            var selectSql = builder.Select<Users>();

            Assert.IsNotNull(selectSql);

            Console.WriteLine(selectSql);
        }

        [TestMethod()]
        public void MethodNameToWhereTest()
        {
            var builder = new SqlBuilder(null);
            var name = "QueryByUserName";
            var where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Console.WriteLine(where);

            name = "QueryByUserNameAndId";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Console.WriteLine(where);
        }
    }
}