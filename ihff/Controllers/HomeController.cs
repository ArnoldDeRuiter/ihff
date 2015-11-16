using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ihff.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Movies()
        {
            ViewBag.Message = "Movies...";

            return View();
        }

        public ActionResult Food()
        {
            ViewBag.Message = "Food...";

            return View();
        }

        public ActionResult Activities()
        {
            ViewBag.Message = "Activities...";

            return View();
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