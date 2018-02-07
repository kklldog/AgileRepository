using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Repository;
using Agile.Repository.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileRepositoryTests.Attributes
{
    [TestClass]
    public class ExecuteTests
    {
        IInterfaceProxy Proxy;
        public ExecuteTests()
        {
            Proxy = Agile.Repository.AgileRepository.Proxy;
            AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                },
            });
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();

            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            instance.Insert(user);

            var result = instance.Execute(user.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result>0);

            Console.WriteLine("Execute result :{0}", result);
        }

    }
}
