using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Agile.Repository;
using Agile.Repository.Proxy;

namespace AgileRepositoryTests.Attributes
{
    [TestClass()]
    public class QueryByTests
    {
        IInterfaceProxy Proxy;
        public QueryByTests()
        {
            Proxy = Agile.Repository.AgileRepository.Proxy;
            Agile.Repository.AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                },
            });
        }

        [TestMethod()]
        public void SqlAttributeTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.TestSql();

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

        [TestMethod()]
        public void SqlAttributeTest1()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.TestSql1("admin");

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

       

        [TestMethod()]
        public void QueryByUserNameTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryByUserName("admin");

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }


        [TestMethod()]
        public void QueryByUserNameAndIdTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryByUserNameAndId("admin", "621BBA76-7496-4486-94A8-08BF9C7EE599");

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }


        [TestMethod()]
        public void QueryByCreaterIsNullTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryByCreaterIsNull();

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

        [TestMethod()]
        public void QueryByCreaterIsNotNullTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryByCreaterIsNotNull();

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

        [TestMethod]
        public void QueryAllTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryAll();

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }
     
    }
}