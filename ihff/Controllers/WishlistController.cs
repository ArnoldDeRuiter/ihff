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
            //List<Item> sessionList = Session["wishList"] as List<Item>; //Get from OrderLine where Code == Code, en dan de Items Get from where ItemId==ItemId            

            if (Session["code"] != null)
            {
                string Code = Session["code"].ToString();

                List<Order> allOrders = new List<Order>();
                allOrders = orderItem.GetOrders(Code);

                foreach (Order o in allOrders)
                {
                    Item it = orderItem.GetItem(o.ItemId);

                    OrderItemCombined combined = new OrderItemCombined();
                    //Order
                    combined.ItemId = o.ItemId;
                    combined.Amount = o.Amount;
                    combined.TotalPrice = o.TotalPrice;
                    combined.WishlistCode = o.WishlistCode;

                    //Item
                    combined.AgeClassification = it.AgeClassification;
                    combined.Cast = it.Cast;
                    combined.DescriptionENG = it.DescriptionENG;
                    combined.DescriptionNL = it.DescriptionNL;
                    combined.Director = it.Director;
                    combined.Length = it.Length;
                    combined.Year = it.Year;
                    combined.DateBegin = it.DateBegin;
                    combined.DateEnd = it.DateEnd;
                    combined.EventType = it.EventType;
                    combined.Image = it.Image;
                    combined.MaxAvailabillity = it.MaxAvailabillity;
                    combined.Name = it.Name;
                    combined.Price = it.Price;
                    if (o.ItemId2 != null)
                        combined.ItemId2 = o.ItemId2;
                    else
                        combined.ItemId2 = null;

                    allCombined.Add(combined);
                }
            }

            return PartialView(allCombined);
        }
                
        
        [HttpPost]
        public ActionResult addWish(int id, int? id2)
        {   
            if (ModelState.IsValid)
            {
                //Code
                if (Session["code"] == null)
                {
                    string code = wishlistRepository.getTempCode();
                    Session["code"] = code;
                }
                string Code = Session["code"].ToString();

                //Wishlist Code
                bool CodeAlready = false;

                if (db.Wishlists.Any(wo => wo.WishlistCode == Code))
                    CodeAlready = true;

                if (!CodeAlready)
                {
                    Wishlist w = new Wishlist();
                    w.WishlistCode = Code;
                    db.Wishlists.Add(w);
                    db.SaveChanges();
                }


                //Orderline bestaand
                List<Order> allOrders = orderItem.GetOrders(Code);
                 bool newItem = true;
                foreach (Order o in allOrders)
                {
                    if (o.ItemId == id) {
                        newItem = false;
                        db.Orderlines.Attach(o);
                        db.Entry(o).State = System.Data.Entity.EntityState.Modified;

                        Models.Item selItem = itemRepository.GetItem(id);

                        o.ItemId = id;
                        o.Amount = (o.Amount + 1);
                        o.TotalPrice = (o.Amount * selItem.Price);
                        o.WishlistCode = Code;
                        if (id2 != null)
                            o.ItemId2 = id2;
                        else
                            o.ItemId2 = null;

                        //db.Orderlines.Add(o);
                        db.SaveChanges();
                    }
                }
                //Orderline nieuw
                if (newItem)
                {                
                    Models.Item selItem = itemRepository.GetItem(id);

                    Order o = new Order();

                    o.ItemId = id;
                    int totAm = (o.Amount + 1);
                    o.Amount = totAm;
                    double? totPr = (o.Amount * selItem.Price);
                    o.TotalPrice = totPr;
                    o.WishlistCode = Code.ToString();
                    if (id2 != null)
                        o.ItemId2 = id2;
                    else
                        o.ItemId2 = null;

                    db.Orderlines.Add(o);
                    db.SaveChanges();
                }
                
                return Redirect(Request.UrlReferrer.ToString());
            }
            return View();
        }

        
        public ActionResult wishUpdateOrder(int hiddenUpdateAmountVal, int hiddenUpdateIDVal)
        {
            //deze methode is tevens delete, indien amount is/word 0
            int amount = hiddenUpdateAmountVal;
            int id = hiddenUpdateIDVal;

            string Code = Session["code"].ToString();
            List<Order> allOrders = orderItem.GetOrders(Code);
            int index = allOrders.FindIndex(it => it.ItemId == id); //Jeroen? ItemId2

            Order o = allOrders[index];
            db.Orderlines.Attach(o);
            db.Entry(o).State = System.Data.Entity.EntityState.Modified;

            Models.Item selItem = itemRepository.GetItem(id);

            o.ItemId = id;
            //o.ItemId2 = null; //jeroen?
            o.Amount = (o.Amount + 1);
            o.TotalPrice = (o.Amount * selItem.Price);
            o.WishlistCode = Code;

            /*db.Orderlines.Add(o);
            db.SaveChanges();*/

            int count = o.Amount;

            if (amount == 0)
            {
                //int offset = count - amount;
                //for (int i = 0; i < offset; i++)
                //{
                    //wishList.RemoveAt(index);
                    db.Orderlines.Remove(o);
                //}
                //Session["wishList"] = wishList;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            
                        
            if (amount <= count)
            {
                int offset = count - amount;
                for (int i = 0; i < offset; i++)
                {
                    //wishList.RemoveAt(index);
                    o.Amount--;
                }
                
            } else
            {
                int offset = amount - count;
                for (int i = 0; i < offset; i++)
                {
                    //wishList.Add(itempje);
                    o.Amount++;
                }                
            }
            //Session["wishList"] = wishList;
            //db.Orderlines.Add(o);
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult wishOrder(string Code, FormCollection form)
        {            
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
                
                int Iteratie = (int.TryParse(form["Iteratie"], out Iteratie)) ? Convert.ToInt32(form["Iteratie"]) : 0;
                for (int i = 1; i <= Iteratie; i++)
                {
                    int ItemId = (int.TryParse(form["ItemId_" + i], out ItemId)) ? Convert.ToInt32(form["ItemId_" + i]) : 0;
                    int ItemId2 = (int.TryParse(form["ItemId2_" + i], out ItemId2)) ? Convert.ToInt32(form["ItemId2_" + i]) : 0;
                    int Amount = (int.TryParse(form["Amount_" + i], out Amount)) ? Convert.ToInt32(form["Amount_" + i]) : 0;
                    double TotalPrice = (double.TryParse(form["TotalPrice_" + i], out TotalPrice)) ? Convert.ToDouble(form["TotalPrice_" + i]) : 0.0;
                    string WishlistCode = form["WishlistCode_" + i].ToString();

                    Order o = new Order();
                    o.ItemId = ItemId;
                    o.ItemId2 = null; //jeroen? ItemId2
                    o.Amount = Amount;
                    o.TotalPrice = TotalPrice;
                    o.WishlistCode = WishlistCode;
                    //db.Orderlines.Add(o);
                }

                db.SaveChanges();
                return RedirectToAction("Index", "Reservation");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult wishReturningCode(string Code)
        {
            if (ModelState.IsValid)
            {
                bool CodeAlready = false;

                if (db.Wishlists.Any(o => o.WishlistCode == Code))
                    CodeAlready = true;

                if (CodeAlready)
                Session["code"] = Code;

                return Redirect(Request.UrlReferrer.ToString());
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