using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers;
using ihff.Models;
using ihff.Controllers.Reposotories;
using System.Net;

namespace ihff.Controllers
{
    public class WishlistController : Controller
    {
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();
        
        // GET: Wishlist
        public ActionResult Index()
        {
            return View();
        }
                
        Models.Item selMovie;
        [HttpPost]
        public ActionResult addWish(int id)
        {
            
            if (ModelState.IsValid)
            {
                //Code
                if (Session["code"] == null)
                {
                    string code = wishlistRepository.getTempCode();
                    Session["code"] = code;
                }

                //Items
                DbItemRepository x = new DbItemRepository();
                selMovie = x.GetItem(id);
                List<Item> addedItems = new List<Item>();
                addedItems.Add(selMovie);

                List<Item> wishList = new List<Item>();
                wishList = (Session["wishList"]!=null) ? Session["wishList"] as List<Item> : wishList;
                wishList.Add(selMovie);
                Session["wishList"] = wishList;
                
                return Redirect(Request.UrlReferrer.ToString());
            }
            return View(selMovie);
        }
        
        public ActionResult wishUpdateOrder(int hiddenUpdateAmountVal, int hiddenUpdateIDVal)
        {

            int amount = hiddenUpdateAmountVal;
            int id = hiddenUpdateIDVal;

            List<Item> wishList = new List<Item>();
            wishList = Session["wishList"] as List<Item>;

            int index = wishList.FindIndex(it => it.ItemId == id);

            Item itempje = wishList[index];

            
            int count = 0;
            foreach (var item in wishList)
            {
                if (item==itempje) {
                    count++;
                }
            }

            if (amount <= count)
            {
                wishList.RemoveAt(index);
            } else
            {
                wishList.Add(itempje);
            }

            

            Session["wishList"] = wishList;

            return Redirect(Request.UrlReferrer.ToString());
        }

        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        [HttpPost]
        [ValidateAntiForgeryToken]
                                    //Code wishlist    //Name //ID  //COUNT //totPrijs
        public ActionResult wishOrder(string Code) //, List<Tuple<string, int, int, double?>> lstTuple
        {
            List<Tuple<string, int, int, double?>> lstTuple = Session["tupleLijst"] as List<Tuple<string, int, int, double?>>;
            if (ModelState.IsValid)
            {
                Wishlist w = new Wishlist();
                w.WishlistCode = Code;
                db.Wishlists.Add(w);
                db.SaveChanges();
                foreach (var t in lstTuple)
                {
                    int tID = t.Item2;
                    double? tTotPrijs = t.Item4;
                    Order o = new Order();
                    o.ItemId = tID;
                    o.TotalPrice = tTotPrijs;
                    o.WishlistCode = Code;
                    db.Orderlines.Add(o);
                }
                
                db.SaveChanges();
                Session["tupleLijst"] = null;
                return RedirectToAction("Index", "Reservation");
            }
            return View();
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