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

        // GET: Reservation
        public ActionResult Index()
        {            
            string code = Session["code"].ToString();
            
            // orders ophalen
            List<Order> allOrders = orderItem.GetOrders(code);

            List<Item> allItems = new List<Item>();
     
            OrderItemCombined combined = new OrderItemCombined();

            List<OrderItemCombined> allCombined = new List<OrderItemCombined>();

            foreach (Order o in allOrders)
            {
                allItems = orderItem.GetItems(o.ItemId);

                combined.ItemId = o.ItemId;
                combined.Amount = o.Amount;
                combined.TotalPrice = o.TotalPrice;
                combined.WishlistCode = o.WishlistCode;

                allCombined.Add(combined);
            }

            foreach (Item i in allItems)
            {
                combined.DateBegin = i.DateBegin;
                combined.DateEnd = i.DateEnd;
                combined.EventType = i.EventType;
                combined.Image = i.Image;
                combined.ItemId = i.ItemId;
                combined.Location = i.Location;
                combined.MaxAvailabillity = i.MaxAvailabillity;
                combined.Name = i.Name;
                combined.Price = i.Price;
                allCombined.Add(combined);
            }

            //foreach (var l in masterTuple)
            //{

            //}


            //ViewBag.allOrders = allOrders;
            //ViewBag.allItems = allItems;
            ViewBag.AllCombined = allCombined;
            
            //arnie code
            //var samen = from q in allOrders
            //            group q by q.ItemId into g
            //            let item = g
            //            select new { IID = g.Key, ITID = item, Itempjes = from a in allItems select a };

            //foreach (var i in samen)
            //{
            //    int itemidorder = i.IID;
            //    var a = i.Itempjes.ToList();
            //    var itemitemid = a[0];
            //    int itemID = itemitemid.ItemId;

            //}
            //ViewBag.ordersMetItems = samen;

            return View(allCombined);
        }

        // GET: Reservation/Details/5
        public ActionResult Details(int? id)
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
        }

        // GET: Reservation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,ReservationCode,WishlistCode,Name")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {

                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("PaymentSucces");
            }

            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public ActionResult Edit(int? id)
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
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,ReservationCode,WishlistCode,Name")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public ActionResult Delete(int? id)
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
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
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
