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
