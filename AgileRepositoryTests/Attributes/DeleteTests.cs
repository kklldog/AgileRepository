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
    public class DeleteTests
    {
        IInterfaceProxy Proxy;
        public DeleteTests()
        {
            Proxy = Agile.Repository.AgileRepository.Proxy;
            AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                }
            });
        }

        [TestMethod]
        public void DeleteTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();
            user.UserName = "test";

            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            instance.Insert(user);

            var result = instance.Delete(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);

            Console.WriteLine("Delete result :{0}", result);
        }

        [TestMethod]
        public void DeleteEntitiesTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();
            user.UserName = "test";
            var user1 = new User();
            user1.CreateTime = DateTime.Now;
            user1.Id = Guid.NewGuid().ToString();
            user1.Password = Guid.NewGuid().ToString();
            user1.UserName = "test";

            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            instance.Insert(user);
            instance.Insert(user1);

            var result = instance.Delete(new List<User>() { user, user1 });

            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);

            Console.WriteLine("Delete result :{0}", result);
        }

        [TestMethod]
        public void DeleteByNameTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = Guid.NewGuid().ToString();
            user.UserName = "deletetestuser";

            var instance = Proxy.CreateProxyInstance<ITestInterface>();
            instance.Insert(user);

            var result = instance.DeleteByUserName(user.UserName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);

            Console.WriteLine("DeleteByName result :{0}", result);
        }

    }
}
