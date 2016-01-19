using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ihff.Models;
using ihff.Controllers.Reposotories;

namespace ihff.Controllers
{
    public class ReservationController : BaseController
    {
        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        private IOrderItemRepository orderItem = new DbOrderItemRepository();
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();

        // GET: Reservation
        public ActionResult Index()
        {
            // nieuwe lijst Orders aanmaken
            List<Order> allOrders = new List<Order>();

            // string code aanmaken
            string code = "";

            // als iemand al z'n order heeft gepslitst
            if (Session["code2"] != null)
            {
                // nieuwe wishlistcode ophalen 
                code = Session["code2"].ToString();
                
                // orders ophalen met nieuwe wishlistcode
                allOrders = orderItem.GetOrders(code);

                if (allOrders.Count == 0)
                {
                    allOrders = orderItem.GetOrders(Session["code"].ToString());
                }

            }
            // nog niet de order gesplitst
            else
            {
                // wishlistcode ophalen zoals het origineel
                code = Session["code"].ToString();

                // orders ophalen
                allOrders = orderItem.GetOrders(code);
            }

            // items ophalen
            List<Item> allItems = new List<Item>();
            
            // lijst van het gecombineerde model aanmaken
            List<OrderItemCombined> allCombined = new List<OrderItemCombined>();

            // voor elke Order in allOrders 
            foreach (Order o in allOrders)
            {
                // haal de info van het item op dmv ItemId
                Item q = orderItem.GetItem(o.ItemId);

                // maak een instantie van het gecombineerde model aan
                OrderItemCombined combined = new OrderItemCombined();  
                
                // combined met de orderinfo vullen
                combined.ItemId = o.ItemId;
                combined.Amount = o.Amount;
                combined.TotalPrice = o.TotalPrice;
                combined.WishlistCode = o.WishlistCode;
                combined.ItemId2 = o.ItemId2;
                string Name = q.Name;
                DateTime realEnding = q.DateEnd;
                double? realPricing = q.Price;
                if (combined.ItemId2 != null)
                {
                    int item2 = (int.TryParse(o.ItemId2.ToString(), out item2)) ? Convert.ToInt32(o.ItemId2) : 0;
                    Item it2 = orderItem.GetItem(item2);

                    //Item
                    Name = q.Name + " & " + it2.Name;
                    realEnding = it2.DateEnd;
                    realPricing = o.TotalPrice;
                }
                // combined met de iteminfo vullen
                combined.DateBegin = q.DateBegin;
                combined.DateEnd = realEnding;
                combined.EventType = q.EventType;
                combined.Image = q.Image;
                combined.Location = q.Location;
                combined.MaxAvailabillity = q.MaxAvailabillity;
                combined.Name = Name;
                combined.Price = realPricing;
                
                // vul de lijst van het gecombineerde model met de instantie van het gecombineerde model
                allCombined.Add(combined);

                //rinse, repeat
            }

            // return lijstje van het gecombineerde model (als het goed is gevuld)
            return View(allCombined);
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, string tel, string paymentMethod)
        {
            // wishlistcode ophalen
            string code = Session["code"].ToString();

            // orders ophalen
            List<Order>allOrders = orderItem.GetOrders(code);

            //  maak reservation instantie aan
            Reservation res = new Reservation();

            // vul reservation instantie met info uit de view
            res.ReservationName = name;
            res.TelNumber = tel;
            res.PaymentMethod = paymentMethod;
            res.WishlistCode = code;

            // todo : schrijf eigen methode om reservationcode aan te maken
            // ik jat nu nog de wishlistrepo.GetTempCode voor de reserverings code
            res.ReservationCode = wishlistRepository.getTempCode();

            // reservering toevoegen
            db.Reservations.Add(res);

            // wishlist legen
            foreach (Order o in allOrders)
            {
                // attach de order aan de db
                db.Orderlines.Attach(o);

                // items ophalen die bij die order horen om zo de MaxAvailabillity te wijzigen
                Item i = orderItem.GetItem(o.ItemId);

                // attach db.items aan item
                db.Items.Attach(i);

                //kijken of item voldoet
                db.Entry(i).State = EntityState.Modified;

                //Change MaxAvailabillity
                i.MaxAvailabillity = (i.MaxAvailabillity - o.Amount);

                // verwijder de order uit de db
                db.Orderlines.Remove(o);

            }
            // sla wijzigingen aan de db op
            db.SaveChanges();

            // laat de payment succes pagina zien en geef 'res' mee voor de res.ReservationCode
            return RedirectToAction("PaymentSucces", res);
            
         }
        
        [HttpPost]
        public ActionResult AlterOrder(OrderItemCombined i, int inputAmount)
        {
            // check of object word meegegeven
            if (i == null)
            {
                // standaardfoutmelding yeey
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            

            // haal Item op via ItemID
            Item item = orderItem.GetItem(i.ItemId);
            int id2 = (int.TryParse(i.ItemId2.ToString(), out id2)) ? id2 : 0;
            

            // pak de order met de juiste wishlistcode en het juiste itemID
            //Order order = orderItem.GetOrder(i.WishlistCode, i.ItemId);
            List<Order> allOrders = orderItem.GetOrders(i.WishlistCode);
            int index = 0;
            if (id2 == 0)
            {
                index = allOrders.FindIndex(it => it.ItemId == i.ItemId);
            }
            else
            {
                Item item2 = orderItem.GetItem(id2);
                index = allOrders.FindIndex(it => it.ItemId == i.ItemId && it.ItemId2 == id2);
            }

            Order order = allOrders[index];



            order.Amount = inputAmount;
            if (id2 != 0)
            {
                //order.ItemId2 = id2;
                order.TotalPrice = (order.Amount * 67.99);
            }
            else
            {
                order.TotalPrice = (order.Amount * item.Price);
            }

            // vul variabele prijspp met prijs van het Item
            //double? prijspp = item.Price;

            // attach order aan de db
            db.Orderlines.Attach(order);
            
            // geef aan dat de status van de entry is aangepast.
            db.Entry(order).State = EntityState.Modified;



            // vul order.Totalprice met het aantal kaartjes * de prijs van het item
            //order.TotalPrice = (inputAmount * prijspp);
            // sla veranderingen in de db op
            db.SaveChanges();

            // laadt de pagina opnieuw
            return Redirect(Request.UrlReferrer.ToString());

        }

        // GET: Reservation/Delete/5
        [HttpPost]
        public ActionResult DeleteOrder(OrderItemCombined i, bool? hiddenDeleteKnopBoolVal)
        {
            // kijken of het object niet null is
            if (i == null)
            {
                // standaard foutmelding..
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // haal de juiste order op dmv wishlistcode en itemId
            Order order = orderItem.GetOrder(i.WishlistCode, i.ItemId);

            // Wil de gebruiker de order bewaren?
            if (hiddenDeleteKnopBoolVal == true)
            {
                // lijst vullen met orders die de orginele wishlistcode hebben
                List<Order> orders = orderItem.GetOrders(Session["code"].ToString());

                string code2 = "";

                // als deze session leeg is
                if (Session["code2"] == null)
                    {
                        // haal dan een nieuwe code op
                        code2 = wishlistRepository.getTempCode();
                        // sla dat in de session op 
                        Session["code2"] = code2;

                    }

                    else
                    {
                        code2 = Session["code"].ToString();
                        
                    }

                //Wishlist Code
                bool CodeAlready = false;

                if (db.Wishlists.Any(wo => wo.WishlistCode == code2))
                    CodeAlready = true;

                if (!CodeAlready)
                {
                    Wishlist w = new Wishlist();
                    w.WishlistCode = code2;
                    db.Wishlists.Add(w);
                    db.SaveChanges();
                }

                foreach (Order o in orders)
                    {
                        db.Orderlines.Attach(o);
                        db.Entry(o).State = EntityState.Modified;
                        o.WishlistCode = Session["code2"].ToString();
                        db.SaveChanges();
                    }

                Order oldOrder = orderItem.GetOrder(Session["code2"].ToString(), i.ItemId);
                
                db.Orderlines.Attach(oldOrder);
                db.Entry(oldOrder).State = EntityState.Modified;
                oldOrder.WishlistCode = Session["code"].ToString();

                db.SaveChanges();

                }

            // !Throw away completely! 
            else
            {
                // attach order aan de db
                db.Orderlines.Attach(order);
                // haal order uit de db
                db.Orderlines.Remove(order);
                db.SaveChanges();
            }
            // !Throw away completely! 
            
            // refresh
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult PaymentSucces (Reservation res)
        {
            // nog even kijken wanneer ik die code2 remove
            Session.Remove("code2");
            // return view PaymentSucces met parameter res
            return View(res);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
