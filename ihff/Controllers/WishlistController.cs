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
    public class WishlistController : BaseController
    {
        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();
        private IItemRepository itemRepository = new DbItemRepository();
        private IOrderItemRepository orderItem = new DbOrderItemRepository();

        //Index word uitgevoerd bij laden pagina.
        public ActionResult Index()
        {
            //List voor Combined Model van Order en Item
            List<OrderItemCombined> allCombined = new List<OrderItemCombined>();

            //Controleren of we al begonnen zijn.
            if (Session["code"] != null)
            {
                //Session in een mooie variabel zetten
                string Code = Session["code"].ToString();
                //Alle orders in een List op basis van Code
                List<Order> allOrders = new List<Order>();
                allOrders = orderItem.GetOrders(Code);
                //Alle orders in de List doorlopen
                foreach (Order o in allOrders)
                {
                    //Pak item van Order
                    Item it = orderItem.GetItem(o.ItemId);
                    //Maak een Combined objectje aan
                    OrderItemCombined combined = new OrderItemCombined();
                    //Order
                    combined.ItemId = o.ItemId;
                    combined.Amount = o.Amount;
                    combined.TotalPrice = o.TotalPrice;
                    combined.WishlistCode = o.WishlistCode;
                    combined.ItemId2 = o.ItemId2;
                    //De volgende variabelen worden mogelijk aangepast indien ItemId2 (ivm F&F) ook gevuld is.
                    string Name = it.Name;
                    DateTime realEnding = it.DateEnd;
                    double? realPricing = it.Price;
                    if (combined.ItemId2 != null)
                    {
                        int item2 = (int.TryParse(o.ItemId2.ToString(), out item2)) ? Convert.ToInt32(o.ItemId2) : 0;
                        Item it2 = orderItem.GetItem(item2);
                        //Item
                        Name = it.Name + " & " + it2.Name;
                        realEnding = it2.DateEnd;
                        realPricing = o.TotalPrice;
                    }
                        //Item
                        combined.AgeClassification = it.AgeClassification;
                        combined.Cast = it.Cast;
                        combined.DescriptionENG = it.DescriptionENG;
                        combined.DescriptionNL = it.DescriptionNL;
                        combined.Director = it.Director;
                        combined.Length = it.Length;
                        combined.Year = it.Year;
                        combined.DateBegin = it.DateBegin;
                        combined.DateEnd = realEnding;// it.DateEnd;
                        combined.EventType = it.EventType;
                        combined.Image = it.Image;
                        combined.MaxAvailabillity = it.MaxAvailabillity;
                        combined.Name = Name;
                        combined.Price = realPricing;//it.Price;
                    //En voeg het toe aan de List.
                    allCombined.Add(combined);
                }
            }
            //PartialView omdat het een PartialView is.
                                //allCombined naar View
            return PartialView(allCombined);
        }
                
        [HttpPost] //Zodat deze methode enkel via een POST kan worden uitgevoerd.
        //Voeg toe aan Wishlist/Maak wishlist en Amount++ indien zelfde item toegevoegd.
        public ActionResult addWish(int id)
        {   
                //Code, kijken of er al een Wishlist sessie loopt eigenlijk.
                if (Session["code"] == null)
                {
                    //En zo niet, aanmaken.
                    string code = wishlistRepository.getTempCode(); //Hierin worden natuurlijk de nodige checks uitgevoerd (geen code maken die al eens eerder is gemaakt)
                    Session["code"] = code;
                }
                string Code = Session["code"].ToString();

                //Kijken of de Code al in de Wishlist tabel staat in de DB.
                bool CodeAlready = false;
                if (db.Wishlists.Any(wo => wo.WishlistCode == Code))
                    CodeAlready = true;
                //Zo niet, zet het er in.
                if (!CodeAlready)
                {
                    Wishlist w = new Wishlist();
                    w.WishlistCode = Code;
                    db.Wishlists.Add(w);
                    db.SaveChanges();
                }
                
                //Orderline bestaand ophalen
                List<Order> allOrders = orderItem.GetOrders(Code);
                bool newItem = true;
                foreach (Order o in allOrders)
                {
                    
                    if (o.ItemId == id && o.ItemId2 == null) {
                        newItem = false; //Controleren of het er al in staat.
                        //En basically, update het vervolgens alleen maar.
                        db.Orderlines.Attach(o);
                        db.Entry(o).State = System.Data.Entity.EntityState.Modified;

                        Models.Item selItem = itemRepository.GetItem(id);

                        o.ItemId = id;
                        o.Amount = (o.Amount + 1);
                        o.TotalPrice = (o.Amount * selItem.Price);
                        o.WishlistCode = Code;

                        db.SaveChanges();
                    }
                }
                //Orderline nieuw
                if (newItem) //Staan het nog er nog niet tussen, voeg toe.
                {                
                    Models.Item selItem = itemRepository.GetItem(id);

                    Order o = new Order();

                    o.ItemId = id;
                    o.ItemId2 = null;
                    int totAm = (o.Amount + 1);
                    o.Amount = totAm;
                    double? totPr = (o.Amount * selItem.Price);
                    o.TotalPrice = totPr;
                    o.WishlistCode = Code.ToString();

                    db.Orderlines.Add(o);
                    db.SaveChanges();
                }
                //Redirect naar pagina waar je vandaan kwam.
                return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost] //Zodat deze methode enkel via een POST kan worden uitgevoerd.
        //Update amount of verwijder - Deze methode is tevens delete, indien amount is 0
        public ActionResult wishUpdateOrder(int hiddenUpdateAmountVal, int hiddenUpdateID1Val, int hiddenUpdateID2Val)
        {
            int amount = hiddenUpdateAmountVal; //meegegeven amount input
            int id1 = hiddenUpdateID1Val; //ID1 van item megegeven
            //indien ID2 van item is mee gegeven, check dat.
            int id2 = (int.TryParse(hiddenUpdateID2Val.ToString(), out id2)) ? hiddenUpdateID2Val : 0; 
            //Zet code in mooie variabel
            string Code = Session["code"].ToString();
            //Orders ophalen op basis van Code
            List<Order> allOrders = orderItem.GetOrders(Code);
            //Index bepalen dmv FindIndex.
            int index = 0;
            if (id2==0) { 
                index = allOrders.FindIndex(it => it.ItemId == id1);
            } else { 
                index = allOrders.FindIndex(it => it.ItemId == id1 && it.ItemId2 == id2);
            }
            //En voila, juist Order object:
            Order o = allOrders[index];
            //Attachen
            db.Orderlines.Attach(o);
            //Update klaar maken
            db.Entry(o).State = System.Data.Entity.EntityState.Modified;
            //Pak bijhoorend item
            Models.Item selItem = itemRepository.GetItem(id1);
            //o.ItemId = id1;
            o.Amount = amount;
            //F&F of geen F&F, basically
            if (id2 != 0)
            {
                //o.ItemId2 = id2;
                //F&F ticket
                o.TotalPrice = (o.Amount * 67.99);
            } else { 
                o.TotalPrice = (o.Amount * selItem.Price);
            }
            o.WishlistCode = Code;
            //Zet o.Amount in leuke variabel
            int count = o.Amount;
            //Niewe amount check, indien 0, verwijder.
            if (amount == 0)
            {
                db.Orderlines.Remove(o);
                //Opslaan in DB (verwijderd)
                db.SaveChanges();
                //Terug waar je vandaan kwam
                return Redirect(Request.UrlReferrer.ToString());
            }
            
            //Indien nieuwe lager is, haal van o.Amount af.
            if (amount <= count)
            {
                int offset = count - amount;
                for (int i = 0; i < offset; i++)
                {
                    o.Amount--;
                }
                
            }//En anders doen we erbij.
            else
            {
                int offset = amount - count;
                for (int i = 0; i < offset; i++)
                {
                    o.Amount++;
                }                
            }
            //Opslaan in DB (updated)
            db.SaveChanges();
            //Terug waar je vandaan kwam
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost] //Zodat deze methode enkel via een POST kan worden uitgevoerd.
        //Plaats Reservartion
        public ActionResult wishOrder(string Code)
        {
            //Kijken of de Code al in de Wishlist tabel staat in de DB.
            bool CodeAlready = false;
            if (db.Wishlists.Any(o => o.WishlistCode == Code))
                CodeAlready = true;
            //Zo niet, zet het er in.
            if (!CodeAlready) { 
                Wishlist w = new Wishlist();
                w.WishlistCode = Code;
                db.Wishlists.Add(w);
                //Opslaan in DB 
                db.SaveChanges();
            }
            //Op naar de Reservation pagina
            return RedirectToAction("Index", "Reservation");
        }

        [HttpPost] //Zodat deze methode enkel via een POST kan worden uitgevoerd.
        [ValidateAntiForgeryToken]//Geen Fogery er tussen, ivm user invoer.
        public ActionResult wishReturningCode(string Code)
        {
            if (ModelState.IsValid) //controleer invoer
            {
                //Kijken of de Code al in de Wishlist tabel staat in de DB.
                bool CodeAlready = false;
                if (db.Wishlists.Any(o => o.WishlistCode == Code))
                    CodeAlready = true;
                //Indien waar, zet de Sessie weer aan:
                if (CodeAlready)
                    Session["code"] = Code;
                //Terug waar je vandaan kwam
                return Redirect(Request.UrlReferrer.ToString());
            }
            //En anders niet.
            return View();   
        }
    }
}