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
        private IItemRepository itemRepository = new DbItemRepository();
        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        //nee        
        private IOrderItemRepository orderItem = new DbOrderItemRepository();
        //ja
        private IOrderRepository orderLine = new DbOrderRepository();

        // GET: Wishlist
        public ActionResult Index()
        {
            List<OrderItemCombined> allCombined = new List<OrderItemCombined>();
            //Code
            
            List<Item> sessionList = Session["wishList"] as List<Item>;
            if (sessionList != null)
            {
                string Code = Session["code"].ToString();
                var q = from x in sessionList
                        group x by x.ItemId into g
                        let count = g.Count()
                        select new { Id = g.Key, Amount = count, All = from a in g select a };

                foreach (var i in q)
                {
                    var a = i.All.ToList()[0];
                    double? prijsItem = ((double?)a.Price * (double?)i.Amount);

                    OrderItemCombined combined = new OrderItemCombined();
                    //Order
                    combined.ItemId = a.ItemId;
                    combined.Amount = i.Amount;
                    combined.TotalPrice = prijsItem;
                    combined.WishlistCode = Code;

                    //Item
                    combined.AgeClassification = a.AgeClassification;
                    combined.Cast = a.Cast;
                    combined.DescriptionENG = a.DescriptionENG;
                    combined.DescriptionNL = a.DescriptionNL;
                    combined.Director = a.Director;
                    combined.Length = a.Length;
                    combined.Year = a.Year;
                    combined.DateBegin = a.DateBegin;
                    combined.DateEnd = a.DateEnd;
                    combined.EventType = a.EventType;
                    combined.Image = a.Image;
                    //combined.ItemId = a.ItemId;
                    combined.MaxAvailabillity = a.MaxAvailabillity;
                    combined.Name = a.Name;
                    combined.Price = a.Price;
                    combined.Price = a.Price;
                    allCombined.Add(combined);
                }
            }
            return PartialView(allCombined);
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
                selMovie = itemRepository.GetItem(id);
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
                if (item.ItemId == itempje.ItemId)
                {
                    count++;
                }
            }


            if (amount == 0)
            {
                int offset = count - amount;
                for (int i = 0; i < offset; i++)
                {
                    wishList.RemoveAt(index);
                }
                Session["wishList"] = wishList;
                return Redirect(Request.UrlReferrer.ToString());
            }
            
                        
            if (amount <= count)
            {
                int offset = count - amount;
                for (int i = 0; i < offset; i++)
                {
                    wishList.RemoveAt(index);
                }
                
            } else
            {
                int offset = amount - count;
                for (int i = 0; i < offset; i++)
                {
                    wishList.Add(itempje);
                }                
            }
            
            Session["wishList"] = wishList;

            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
                                    //Code wishlist    //Name //ID  //COUNT //totPrijs
        public ActionResult wishOrder(string Code) //, List<Tuple<string, int, int, double?>> lstTuple
        {
            List<Tuple<string, int, int, double?>> lstTuple = Session["tupleLijst"] as List<Tuple<string, int, int, double?>>;
            if (ModelState.IsValid)
            {
                bool CodeAlready = false;

                if (db.Wishlists.Any(o => o.WishlistCode == Code))
                    CodeAlready = true;

                if (!CodeAlready) { 
                    Wishlist w = new Wishlist();
                    w.WishlistCode = Code;
                    db.Wishlists.Add(w);
                    db.SaveChanges();
                }

                foreach (var t in lstTuple)
                {
                    int tID = t.Item2;
                    double? tTotPrijs = t.Item4;
                    Order o = new Order();
                    o.ItemId = tID;
                    o.Amount = t.Item3;
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