using System;
using Agile.Repository.Sql;
using AgileRepositoryTests.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileRepositoryTests.Sql
{
    public class Users
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }

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
                " UserName=@UserName");
            Console.WriteLine(where);

            name = "QueryByUserNameAndId";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName=@UserName And Id=@Id");
            Console.WriteLine(where);

            name = "QueryByUserNameAndIdOrNickNameAndChineseName";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName=@UserName And Id=@Id Or NickName=@NickName And ChineseName=@ChineseName");
            Console.WriteLine(where);

            name = "QueryByUserNameAndIdOrNickNameAndChineseNameAndSexIsNullOrAgeIsNotNull";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName=@UserName And Id=@Id Or NickName=@NickName And ChineseName=@ChineseName And Sex Is Null Or Age Is Not Null");
            Console.WriteLine(where);

            name = "CountByUserName";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName=@UserName");
            Console.WriteLine(where);

            name = "CountByUserNameAndId";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName=@UserName And Id=@Id");
            Console.WriteLine(where);

            name = "CountByUserNameGreaterThenAndIdLessThenOrNumberGreaterEqualOrAgeLessEqual";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName>@UserName And Id<@Id Or Number>=@Number Or Age<=@Age");
            Console.WriteLine(where);

            name = "CountByUserNameNotAndId";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName!=@UserName And Id=@Id");
            Console.WriteLine(where);

            name = "CountByUserNameInOrIdNotIn";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName In @UserName Or Id Not In @Id");
            Console.WriteLine(where);

            name = "CountByUserNameLike";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName Like @UserName");
            Console.WriteLine(where);

            name = "CountByUserNameNotLike";
            where = builder.MethodNameToWhere(name);
            Assert.IsNotNull(where);
            Assert.AreEqual(where,
                " UserName Not Like @UserName");
            Console.WriteLine(where);
        }
    }
}