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
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();
        // GET: Wishlist
        public ActionResult Index()
        {
            if (Session["code"] == null)
            {
                Session["code"] = wishlistRepository.getTempCode();
            }
            return View();
        }
                
        Models.Item selMovie;
        [HttpPost]
        public ActionResult addWish(int id)
        {
            if (ModelState.IsValid)
            {
                DbItemRepository x = new DbItemRepository();
                selMovie = x.GetItem(id);
                List<Item> addedItems = new List<Item>();
                addedItems.Add(selMovie);

                List<Item> wishList = new List<Item>();
                wishList = (Session["wishList"]!=null) ? Session["wishList"] as List<Item> : wishList;
                wishList.Add(selMovie);
                Session["wishList"] = wishList;

                return RedirectToAction("Movies", "Home");
            }
            return View(selMovie);
        }
        
        public ActionResult deleteWishlistItem(int id)
        {
            int verkregenID = id; //yay
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