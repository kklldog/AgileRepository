using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgileRepository.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository;
using Agile.Repository.Proxy;
using Agile.Repository.Sql;
using AspectCore.DynamicProxy;

namespace AgileRepository.Proxy.Tests
{
    public class SampleInterceptor : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("call interceptor");
            return context.Invoke(next);
        }
    }

    public class Users
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }

    public interface TestInterface : IAgileRepository
    {
        [SampleInterceptor]
        string Test(string p1);

        [AutoSql(Sql = "SELECT * FROM USERS")]
        IEnumerable<Users> TestSql();

        [AutoSql(Sql = "SELECT * FROM USERS where username=@userName")]
        IEnumerable<Users> TestSql1(string userName);

        [AutoSql]
        IEnumerable<Users> QueryByUserName(string userName);

        [AutoSql]
        IEnumerable<Users> QueryByUserNameAndId(string userName,string id);
    }

    [TestClass()]
    public class InterfaceProxyTests
    {
        [TestMethod()]
        public void InvokeTest()
        {
            var proxy = new InterfaceProxy();
            proxy.Init(null);

            var instance = proxy.CreateProxyInstance<TestInterface>();

            instance.Test("x");

            proxy.Invoke<TestInterface, string>("Test", "x");
        }

        [TestMethod()]
        public void SqlAttributeTest()
        {
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.CreateProxyInstance<TestInterface>();

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
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.CreateProxyInstance<TestInterface>();

            var result = instance.TestSql1("admin");

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

        [TestMethod()]
        public void CreateProxyInstanceTest()
        {
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.CreateProxyInstance<TestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance.GetType());
            var instance1 = proxy.CreateProxyInstance<TestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance1.GetType());
            Assert.AreNotSame(instance, instance1);
        }

        [TestMethod()]
        public void QueryByUserNameTest()
        {
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.CreateProxyInstance<TestInterface>();
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
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.CreateProxyInstance<TestInterface>();
            var result = instance.QueryByUserNameAndId("admin", "621BBA76-7496-4486-94A8-08BF9C7EE599");

            Assert.IsNotNull(result);
            foreach (var user in result)
            {
                Console.WriteLine("id:{0} name:{1}", user.Id, user.UserName);
            }
        }

        [TestMethod()]
        public void GetInstanceTest()
        {
            var proxy = new InterfaceProxy();
            proxy.Init(new AgileRepositoryConfig()
            {
                AssbemlyName = "AgileRepositoryTests"
            });

            var instance = proxy.GetInstance<TestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance.GetType());
            var instance1 = proxy.GetInstance<TestInterface>();
            Assert.IsNotNull(instance1);
            Console.WriteLine(instance1.GetType());
            Assert.AreSame(instance, instance1);
        }
    }
}