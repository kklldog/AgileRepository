﻿using System;
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
    public class InsertTests
    {
        IInterfaceProxy Proxy;
        public InsertTests()
        {
            Proxy = AgileRepository.Proxy;
            AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                },
            });
        }

        [TestMethod]
        public void InsertTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = "12345";
            user.UserName = Guid.NewGuid().ToString();
            var instance = Proxy.CreateProxyInstance<ITestInterface>();

            var result = instance.Insert(user);

            Assert.IsNotNull(result);

            Assert.IsTrue(result > 0);

            Console.WriteLine("insert result :{0}", result);
        }


        [TestMethod]
        public void InsertEntitiesTest()
        {
            var user = new User();
            user.CreateTime = DateTime.Now;
            user.Id = Guid.NewGuid().ToString();
            user.Password = "12345";
            user.UserName = Guid.NewGuid().ToString();
            var user1 = new User();
            user1.CreateTime = DateTime.Now;
            user1.Id = Guid.NewGuid().ToString();
            user1.Password = "12345";
            user1.UserName = Guid.NewGuid().ToString();

            var instance = Proxy.CreateProxyInstance<ITestInterface>();

            var result = instance.Insert(new List<User>() { user, user1 });

            Assert.IsNotNull(result);

            Assert.IsTrue(result > 0);

            Console.WriteLine("insert result :{0}", result);
        }
    }
}
