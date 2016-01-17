using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Models;
using ihff.Controllers;
using ihff.Controllers.Reposotories;

namespace ihff.Controllers
{
    public class HomeController : Controller
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
    }
}