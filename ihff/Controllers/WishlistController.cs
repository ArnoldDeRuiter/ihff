using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ihff.Controllers
{
    public class WishlistController : Controller
    {
        private IWishlistRepository wishlistRepository = new InMemoryWishlistRepository();

        // GET: Wishlist
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Item model)
        {
            if (ModelState.IsValid)
            {
                wishlistRepository.Add(model);
                DbItemRepository x = new DbItemRepository();

                List<Models.Item> lstY = new List<Models.Item>();
                lstY = x.GetAllMovies().ToList();

                return RedirectToAction("addWisual", new { id = model.ItemId });//ja nee moet iets van repository krijgen :/
            }
            return View(model);
        }

        public ActionResult addWisual()
        {
            
            return RedirectToAction("Movies", "Home");
        }

        /*
        //terwijl dit v zou voldoen: moet er ^ gebeuren :p
        public void savewish(Models.Item miForm)
        {
            //string id = form["itemId"];
            int id = miForm.ItemId;
        }
        */
    }
}