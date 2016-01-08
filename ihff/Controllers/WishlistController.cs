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

                //Prijs
                double? totalPrijs = 0.0;
                foreach (var item in wishList)
                {
                    totalPrijs += item.Price;
                }
                Session["totalPrijs"] = totalPrijs;

                return RedirectToAction("Movies", "Home");
            }
            return View(selMovie);
        }
        
        public ActionResult deleteWishlistItem(int id)
        {
            int verkregenID = id; //yay //oke dat komt later dan nog wel... Uit List verwijderen en hoppa..
            //Of amount daarvan in die ene list aanpassen, maar dan is het niet Delete maar changeAmountWishlistItem(int id)
            return RedirectToAction("Movies", "Home");
        }
        


        private IHFFdatabasecontext db = new IHFFdatabasecontext();

        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }*/
        // POST: Reservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
                                    //Code wishlist    //Name //ID  //COUNT //totPrijs
        public ActionResult wishOrder(string Code, List<Tuple<string, int, int, double?>> lstTuple)
        {
            Wishlist w = new Wishlist();
            w.WishlistCode = Code;
            if (ModelState.IsValid)
            {
                db.Wishlists.Add(w);

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