using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgileRepository.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository;
using Agile.Repository.Attributes;
using Agile.Repository.Proxy;
using Agile.Repository.Sql;
using AgileRepositoryTests.Proxy;
using AspectCore.DynamicProxy;

namespace AgileRepository.Proxy.Tests
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
                SqlMonitor = (sql, sender) =>
                {
                    Console.WriteLine(sql);
                }
            });
        }

        [TestMethod()]
        public void CreateProxyInstanceTest()
        {
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance.GetType());
            var instance1 = Proxy.CreateProxyInstance<ITestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance1.GetType());
            Assert.AreNotSame(instance, instance1);
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
            var proxy = new InterfaceProxy();

            var instance = proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.QueryByCreaterIsNotNull();

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

       
        [TestMethod()]
        public void GetInstanceTest()
        {
            var instance = Proxy.GetInstance<ITestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance.GetType());
            var instance1 = Proxy.GetInstance<ITestInterface>();
            Assert.IsNotNull(instance1);
            Console.WriteLine(instance1.GetType());
            Assert.AreSame(instance, instance1);
        }
    }
}