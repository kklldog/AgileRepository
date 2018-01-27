using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agile.Repository;
using AgileRepositoryWeb.IRepository;
using Agile.Repository.Proxy;

namespace AgileRepositoryWebTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IUserRepository repository = AgileRepository.Proxy.GetInstance<IUserRepository>();
            var users = repository.QueryBySql("admin");

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