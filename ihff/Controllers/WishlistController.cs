using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers;
using ihff.Models;
using ihff.Controllers.Reposotories;

namespace ihff.Controllers
{
    public class WishlistController : Controller
    {
        private IWishlistRepository wishlistRepository = new InMemoryWishlistRepository();


        private IItemRepository itemRepository = new DbItemRepository();
        // GET: Wishlist
        public ActionResult Index()
        {
            IEnumerable<Item> allFilms = itemRepository.GetAllItems();
            return View(allFilms.OrderBy(i => i.Name));
        }

      

        Models.Item selMovie;
        [HttpPost]
        public ActionResult addWish(int id) //Models.Item model
        {
            if (ModelState.IsValid)
            {
                //   wishlistRepository.Add(model);
                DbItemRepository x = new DbItemRepository();

                List<Models.Item> lstY = new List<Models.Item>();
                //lstY = x.GetAllMovies().ToList();

                selMovie = x.GetItem(id);

                return RedirectToAction("addWisual", new { movie = selMovie });//ja nee moet iets van repository krijgen :/
            }
            return View(selMovie);//model
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