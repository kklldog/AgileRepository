using System;
using Agile.Repository;
using Agile.Repository.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileRepositoryTests.Attributes
{
    [TestClass()]
    public class CountByTests
    {
        IInterfaceProxy Proxy;
        public CountByTests()
        {
            Proxy = Agile.Repository.AgileRepository.Proxy;
            Agile.Repository.AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters ) =>
                {
                    Console.WriteLine(sql);
                },
                ConnectionName = "conn"
            });
        }

        [TestMethod()]
        public void CountBySqlTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.TestCount();

            Assert.IsNotNull(result);
            Console.WriteLine("count:{0}",result);
        }

        [TestMethod()]
        public void CountBySqlUserNameTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.TestCount1("admin");

            Assert.IsNotNull(result);
            Console.WriteLine("count:{0}", result);
        }

        [TestMethod()]
        public void CountByUserNameTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.CountByUserName("admin");

            Assert.IsNotNull(result);
            Console.WriteLine("count:{0}", result);
        }
        [TestMethod()]
        public void CountByUserNameAndIdTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.CountByIdAndUserName("621BBA76-7496-4486-94A8-08BF9C7EE599", "admin");

            Assert.IsNotNull(result);
            Console.WriteLine("count:{0}", result);
        }

        [TestMethod]
        public void CountAllTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.CountAll();

            Assert.IsNotNull(result);
            Console.WriteLine("count:{0}", result);
        }

    }
}