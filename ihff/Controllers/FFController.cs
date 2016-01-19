using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class FFController : BaseController
    {
        private IItemRepository itemRepository = new DbItemRepository();
        private IOrderRepository orderRepository = new DbOrderRepository();
        private IWishlistRepository wishlistRepository = new DbWishlistRepository();
        private IHFFdatabasecontext db = new IHFFdatabasecontext();
        private IOrderItemRepository orderItem = new DbOrderItemRepository();

        // GET: FF
        public ActionResult Index(int Id)
        {
            //Item ophalen om alle diners te tonen op dezelfde dag.
            Item item = itemRepository.GetItem(Id);
            IEnumerable<Item> diners = itemRepository.GetDinerDay(item.DateBegin);

            ViewBag.item1 = item;

            return View(diners);
        }

        //Toevoegen van FFTicket aan Orderline/Wishlist.
        public ActionResult AddFFTicket(int Id1, int Id2)
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
                    if (o.ItemId == Id1 && o.ItemId2 == Id2)
                    {
                        newItem = false;
                        db.Orderlines.Attach(o);
                        db.Entry(o).State = System.Data.Entity.EntityState.Modified;

                        Models.Item selItem = itemRepository.GetItem(Id1);

                        o.ItemId = Id1;
                        o.Amount = (o.Amount + 1);
                        o.TotalPrice = (o.Amount * 67.99);
                        o.WishlistCode = Code.ToString();
                        o.ItemId2 = Id2;

                        db.SaveChanges();
                    }
                }
                //Orderline nieuw
                if (newItem)
                {
                    Models.Item selItem = itemRepository.GetItem(Id1);

                    Order o = new Order();

                    o.ItemId = Id1;
                    int totAm = (o.Amount + 1);
                    o.Amount = totAm;
                    double? totPr = (o.Amount* 67.99);
                    o.TotalPrice = totPr;
                    o.WishlistCode = Code.ToString();
                    o.ItemId2 = Id2;

                    db.Orderlines.Add(o);
                    db.SaveChanges();
                }
                return RedirectToAction("Movies", "Home");
            }
            return View();
        }
    }
}