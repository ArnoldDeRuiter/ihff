using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Models;
using ihff.Controllers;
using ihff.Controllers.Helper;
using ihff.Controllers.Reposotories;

namespace ihff.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        
        private IItemRepository itemRepository = new DbItemRepository();

        public ActionResult Movies()
        {
            ViewBag.Message = "Movies";

            IEnumerable<Item> allFilms = itemRepository.GetAllMovies();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult Food()
        {
            ViewBag.Message = "Food";

            IEnumerable<Item> allFilms = itemRepository.GetAllDiners();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult Activities()
        {
            ViewBag.Message = "Activities";

            IEnumerable<Item> allFilms = itemRepository.GetAllSpecials();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult FoodFilm()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult ChangeCurrentCulture(int id)
        {
            //
            // Change the current culture for this user.
            //
            CultureSessionManager.CurrentCulture = id;
            //
            // Cache the new current culture into the user HTTP session. 
            //
            Session["CurrentCulture"] = id;
            //
            // Redirect to the same page from where the request was made! 
            //
            return RedirectToAction("Index");
        }
    }
}