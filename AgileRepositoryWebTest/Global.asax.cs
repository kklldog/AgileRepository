using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Agile.Repository;
using Agile.Repository.Autofac;
using Autofac;

namespace AgileRepositoryWebTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterAgileRepositories("AgileRepositoryWebTest.IRepository");
            Autofac.Container = builder.Build();

            AgileRepository.SetConfig(new AgileRepositoryConfig()
            {
                SqlMonitor = (sql, paramters) =>
                {
                    Console.WriteLine(sql);
                }
            });
        }
    }
}
