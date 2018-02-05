using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agile.Repository;
using AgileRepositoryWebTest.IRepository;
using Agile.Repository.Proxy;
using Autofac;

namespace AgileRepositoryWebTest.Controllers
{
    public class HomeController : Controller
    {
        protected IUserRepository UserRepository;

        public ActionResult Index()
        {
            IUserRepository repository = Autofac.Container.Resolve<IUserRepository>();
            var users = repository.QueryByUserName("admin");

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}