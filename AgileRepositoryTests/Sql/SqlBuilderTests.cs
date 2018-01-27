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
            var builder = new SqlserverBuilder();

            var selectSql = builder.Select<Users>();

            Assert.IsNotNull(selectSql);

            Console.WriteLine(selectSql);
        }

        [TestMethod()]
        public void MethodNameToWhereTest()
        {
            var builder = new SqlserverBuilder();
            var name = "QueryByUserName";
            var where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " where UserName=@UserName");
            Console.WriteLine(where);

            name = "QueryByUserNameAndId";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " where UserName=@UserName And Id=@Id");
            Console.WriteLine(where);

            name = "QueryByUserNameAndIdOrNickNameAndChineseName";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " where UserName=@UserName And Id=@Id Or NickName=@NickName And ChineseName=@ChineseName");
            Console.WriteLine(where);
        }
    }
}