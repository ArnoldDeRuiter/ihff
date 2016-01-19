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

                // als de nieuwe order lijst leeg is (Zou in theorie niet mogen)
                if (allOrders.Count == 0)
                {
                    // haal dan de originele wishlist op. (die dan als t goed is wel gevuld zou moeten zijn)
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
                // variabelen maken vullen met Item 'q' voor het opslaan van juiste items als itemID 2 is gevuld (dus FF ticket)
                string name = q.Name;
                // pak het einde van de film tijd
                DateTime realEnding = q.DateEnd;
                double? realPricing = q.Price;

                // in het geval dat itemId 2 niet null is en het dus een FF ticket is.
                if (combined.ItemId2 != null)
                {
                    // arnold? waarom is dit zo lang? kan dit niet worden : zie volgende regel
                    // Item it3 = orderItem.GetItem(Convert.ToInt32(o.ItemId2));
                    int item2 = (int.TryParse(o.ItemId2.ToString(), out item2)) ? Convert.ToInt32(o.ItemId2) : 0;

                    Item it2 = orderItem.GetItem(item2);

                    // Item Name variabele is item1 name plus & plus item2 name
                    name = q.Name + " & " + it2.Name;

                    // realEnding variabele vullen met item2 dateEnd
                    realEnding = it2.DateEnd;

                    // realpricing FF ticket itemID1 totaalprijs
                    realPricing = o.TotalPrice;
                }

                // combined met de iteminfo vullen
                combined.DateBegin = q.DateBegin;
                combined.DateEnd = realEnding;
                combined.EventType = q.EventType;
                combined.Image = q.Image;
                combined.Location = q.Location;
                combined.MaxAvailabillity = q.MaxAvailabillity;
                combined.Name = name;
                combined.Price = realPricing;

                // vul de lijst van het gecombineerde model met de instantie van het gecombineerde model
                allCombined.Add(combined);

            }

            // return lijstje van het gecombineerde model (als het goed is gevuld)
            return View(allCombined);
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, string tel, string paymentMethod)
        {
            string code = "";

            if (Session["code2"] != null)
            {
                // wishlistcode ophalen
                code = Session["code2"].ToString();
            }
            else
            {
                // wishlistcode ophalen
                code = Session["code"].ToString();
            }
            // orders ophalen
            List<Order> allOrders = orderItem.GetOrders(code);

            //  maak reservation instantie aan
            Reservation res = new Reservation();

            // vul reservation instantie met info uit de view
            res.ReservationName = name;
            res.TelNumber = tel;
            res.PaymentMethod = paymentMethod;
            res.WishlistCode = code;

            // get reserverings code dmv wishlist code generator
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

            // arnold? waarom is dit zo lang? kan dit niet worden : zie volgende regel
            // Item it3 = orderItem.GetItem(Convert.ToInt32(o.ItemId2));
            Item item = orderItem.GetItem(i.ItemId);
            int id2 = (int.TryParse(i.ItemId2.ToString(), out id2)) ? id2 : 0;

            // lijst allOrders vullen met Orders 
            List<Order> allOrders = orderItem.GetOrders(i.WishlistCode);

            // index aanmaken
            int index = 0;

            // kijken of id2 niet null is
            if (id2 == 0)
            {
                // zoek index op
                index = allOrders.FindIndex(it => it.ItemId == i.ItemId);
            }

            // id2 niet null
            else
            {
                // vul item2 dan dmv id2 met de methode orderitem.GetItem
                Item item2 = orderItem.GetItem(id2);
                // vul dan de index met de index van de order waar itemid en itemid2 allebij zijn
                index = allOrders.FindIndex(it => it.ItemId == i.ItemId && it.ItemId2 == id2);
            }
            // order is de order met de juiste index van de lijst allOrders
            Order order = allOrders[index];

            // hoeveelheid kaartjes is innputAmount
            order.Amount = inputAmount;

            // nu snap k t niet. Dit is toch de else van hierboven?
            if (id2 != 0)
            {
                //order totalprice is 67.99 (vaste prijs) * het aantal kaarten
                order.TotalPrice = (order.Amount * 67.99);
            }
            else
            {
                //order totalprice is aantal kaarten * itemprijs
                order.TotalPrice = (order.Amount * item.Price);
            }

            // attach order aan de db
            db.Orderlines.Attach(order);

            // geef aan dat de status van de entry is aangepast.
            db.Entry(order).State = EntityState.Modified;

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
                List<Order> orders = new List<Order>();

                string code2 = "";

                // als deze session leeg is
                if (Session["code2"] == null)
                {
                    // haal dan een nieuwe code op
                    code2 = wishlistRepository.getTempCode();
                    // sla dat in de session op 
                    Session["code2"] = code2;

                    // lijst vullen met orders die de orginele wishlistcode hebben
                    orders = orderItem.GetOrders(Session["code"].ToString());

                    // bool codealready aanmaken op false zetten
                    bool CodeAlready = false;

                    // als de wishlist tabel een instantie bevat van de wishlist code
                    if (db.Wishlists.Any(wo => wo.WishlistCode == code2))
                        CodeAlready = true;

                    // als de codeAlready bool niet true is
                    if (!CodeAlready)
                    {
                        // maak een nieuwe instantie van wishlist aan
                        Wishlist w = new Wishlist();
                        // vul die instantie van wishlist met met de code
                        w.WishlistCode = code2;
                        // add nieuwe row in de tabel met de info uit wishlist w
                        db.Wishlists.Add(w);
                        // sla veranderingen op 
                        db.SaveChanges();
                    }

                    // voor elke order in de lijst orders
                    foreach (Order o in orders)
                    {
                        // koppel o aan de db in de tabel Orderlines
                        db.Orderlines.Attach(o);
                        // laat weten dat o veranderd word
                        db.Entry(o).State = EntityState.Modified;
                        // verander de wishlistcode van code naar code2
                        o.WishlistCode = Session["code2"].ToString();
                        // sla veranderingen op
                        db.SaveChanges();
                    }

                    // maak een instantie voor de oude order met de order die wishlistcode2 en i.itemID heeft
                    Order oldOrder = orderItem.GetOrder(Session["code2"].ToString(), i.ItemId);
                    // koppel oldOrder aan de db in de tabel Orderlines
                    db.Orderlines.Attach(oldOrder);
                    // laat weten dan oldOrder is aangepast
                    db.Entry(oldOrder).State = EntityState.Modified;
                    // oude wishlistcode toevoegen aan oude order
                    oldOrder.WishlistCode = Session["code"].ToString();
                    // sla wijzigingen op 
                    db.SaveChanges();

                }

                // als code2 niet null is
                else
                {
                    // attach dan de order die mee word gegeven                   
                    db.Orderlines.Attach(order);
                    // geef aan dat order in de db word aangepast
                    db.Entry(order).State = EntityState.Modified;
                    // order krijgt nu wishlist code
                    order.WishlistCode = Session["code"].ToString();
                    // sla wijzigingen op
                    db.SaveChanges();

                }

            }

            // !Throw away completely! 
            else
            {
                // attach order aan de db
                db.Orderlines.Attach(order);
                // haal order uit de db
                db.Orderlines.Remove(order);
                // sla wijzigingen op
                db.SaveChanges();
            }
            // !Throw away completely! 

            // refresh
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult PaymentSucces(Reservation res)
        {
            // nog even kijken wanneer ik die code2 remove
            Session.Remove("code2");
            // return view PaymentSucces met parameter res
            return View(res);
        }

        // Sluit de connectie met de DB automatisch gegenereerd
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
