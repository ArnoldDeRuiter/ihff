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

            

            List<OrderItemCombined> allCombined = new List<OrderItemCombined>();

            foreach (Order o in allOrders)
            {
                Item q = orderItem.GetItem(o.ItemId);

                OrderItemCombined combined = new OrderItemCombined(); //< nogal cruciaal dat deze in je loop staat :3 
                //Order
                combined.ItemId = o.ItemId;
                combined.Amount = o.Amount;
                combined.TotalPrice = o.TotalPrice;
                combined.WishlistCode = o.WishlistCode;

                //Item
                combined.DateBegin = q.DateBegin;
                combined.DateEnd = q.DateEnd;
                combined.EventType = q.EventType;
                combined.Image = q.Image;
                combined.Location = q.Location;
                combined.MaxAvailabillity = q.MaxAvailabillity;
                combined.Name = q.Name;
                combined.Price = q.Price;
                
                allCombined.Add(combined);
            }

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
