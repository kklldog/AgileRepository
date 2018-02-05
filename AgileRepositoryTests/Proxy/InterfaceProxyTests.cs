using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository.Proxy;
using AgileRepositoryTests.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileRepositoryTests.Proxy
{
    [TestClass]
    public class InterfaceProxyTests
    {
        IInterfaceProxy Proxy;
        public InterfaceProxyTests()
        {
            Proxy = new InterfaceProxy();
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
        public void SingletonInstanceTest()
        {
            var instance = Proxy.SingletonInstance<ITestInterface>();
            Assert.IsNotNull(instance);
            Console.WriteLine(instance.GetType());
            var instance1 = Proxy.SingletonInstance<ITestInterface>();
            Assert.IsNotNull(instance1);
            Console.WriteLine(instance1.GetType());
            Assert.AreSame(instance, instance1);
        }

        [TestMethod()]
        public void GetProxyTypeTest()
        {
            var type = Proxy.GetProxyType<ITestInterface>();
            Assert.IsNotNull(type);
            Console.WriteLine(type);
            var type1 = Proxy.GetProxyType<ITestInterface>();
            Assert.IsNotNull(type);
            Console.WriteLine(type);

            Assert.AreEqual(type1,type);
        }
    }
}
