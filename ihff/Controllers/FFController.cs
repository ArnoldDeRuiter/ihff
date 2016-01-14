using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class FFController : Controller
    {
        private IItemRepository itemRepository = new DbItemRepository();
        private IOrderRepository orderRepository = new DbOrderRepository();
        // GET: FF
        public ActionResult Index(int Id)
        {
            Item item = itemRepository.GetItem(Id);
            IEnumerable<Item> diners = itemRepository.GetDinerDay(item.DateBegin);

            ViewBag.item1 = item;

            return View(diners);
        }

        public ActionResult AddFFTicket(int Id, Item movie)
        {
            Item food = itemRepository.GetItem(Id);
            double price = 64.99;

/*            orderRepository.AddOrder(price)    Ben ermee bezig maar ga even commiten =D*/

            return RedirectToAction("Index", "Home");
        }
    }
}