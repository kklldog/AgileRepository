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
    public class UpdateTests
    {
        IInterfaceProxy Proxy;
        public UpdateTests()
        {
            Proxy = AgileRepository.Proxy;
            AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                }
            });
        }

        [TestMethod]
        public void UpdateTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = "621BBA76-7496-4486-94A8-08BF9C7EE599";
            user.Password = Guid.NewGuid().ToString();
            user.UserName = "admin";
            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            var result = instance.Update(user);
            Assert.IsNotNull(result);
            Assert.IsTrue(result>0);

            Console.WriteLine("Update result :{0}",result);
        }
    }
}
