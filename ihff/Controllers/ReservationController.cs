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
    public class ReservationController : Controller
    {
        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        private IOrderItemRepository orderItem = new DbOrderItemRepository();
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();

        // GET: Reservation
        public ActionResult Index()
        {
            // wishlistcode ophalen
            string code = Session["code"].ToString();
            
            // orders ophalen
            List<Order> allOrders = orderItem.GetOrders(code);

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

                // combined met de iteminfo vullen
                combined.DateBegin = q.DateBegin;
                combined.DateEnd = q.DateEnd;
                combined.EventType = q.EventType;
                combined.Image = q.Image;
                combined.Location = q.Location;
                combined.MaxAvailabillity = q.MaxAvailabillity;
                combined.Name = q.Name;
                combined.Price = q.Price;
                
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
            // haal wishlistcode uit sessie
            string code = Session["code"].ToString();

            // lijst met orders om weg te gooien na het maken van de reservering
            List<Order> allOrders = orderItem.GetOrders(code);

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

            // pak de order met de juiste wishlistcode en het juiste itemID
            Order order = orderItem.GetOrder(i.WishlistCode, i.ItemId);

            // haal Item op via ItemID
            Item item = orderItem.GetItem(i.ItemId);
            // vul variabele prijspp met prijs van het Item
            double? prijspp = item.Price;

            // attach order aan de db
            db.Orderlines.Attach(order);
            
            // geef aan dat de status van de entry is aangepast.
            db.Entry(order).State = EntityState.Modified;
            
            // vul order.Amount met de amountinput
            order.Amount = inputAmount;
            // vul order.Totalprice met het aantal kaartjes * de prijs van het item
            order.TotalPrice = (inputAmount * prijspp);
            // sla veranderingen in de db op
            db.SaveChanges();

            // laadt de pagina opnieuw
            return Redirect(Request.UrlReferrer.ToString());

        }

        // GET: Reservation/Delete/5
        [HttpPost]
        public ActionResult DeleteOrder(OrderItemCombined i, bool hiddenDeleteKnopBoolVal)
        {
            // kijken of het object niet null is
            if (i == null)
            {
                // standaard foutmelding..
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // haal de juiste order op dmv wishlistcode en itemId
            Order order = orderItem.GetOrder(i.WishlistCode, i.ItemId);

            // TODO TODO TODO TODO
            // if de bool van js van arnold is true !Keep in wishlist!
            // dan wil ik de bestaande List<order> ophalen en daarvan voor elke order de wishlistcode veranderen
            // wishlist session code blijft dan de code voor het te verwijderen item en nieuwe session list code2 word voor de nieuwe reservation List<order> wishcode
            // Zoniet dan onderstaande code uitvoeren

            List<Order> orders = orderItem.GetOrders(i.WishlistCode);

            

            // !Throw away completely! 

            // attach order aan de db
            db.Orderlines.Attach(order);
            // haal order uit de db
            db.Orderlines.Remove(order);
            
            // !Throw away completely! 

            //sla wijzigingen op
            db.SaveChanges();

            // refresh
            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult PaymentSucces (Reservation res)
        {
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
