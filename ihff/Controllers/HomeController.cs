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
            IEnumerable<Item> allFilms = itemRepository.GetAllMovies();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult Food()
        {
            IEnumerable<Item> allFilms = itemRepository.GetAllDiners();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult Activities()
        {
            IEnumerable<Item> allFilms = itemRepository.GetAllSpecials();

            return View(allFilms.OrderBy(i => i.DateBegin));
        }

        public ActionResult FoodFilm()
        {
            return View();
        }

        public ActionResult Haarlem()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ChangeCurrentCulture(int id)
        {
            // Zet de culture (taal) van de huidige gebruiker
            CultureSessionManager.CurrentCulture = id;

            // Zet de culture in een session
            Session["CurrentCulture"] = id;

            // Redirect naar vorige pagina
            // Zo lijkt de gebruiker op de pagina te blijven terwijl de taal gewijzigd wordt
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
    }
}