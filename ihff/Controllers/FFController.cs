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

        public ActionResult AddFFTicket(int Id1, int Id2)
        {
           // Item food = itemRepository.GetItem(Id); Dit kan weg denk ik, toch niet nodig lol.
            double price = 67.99;
            string wishCode = (string)Session["code"];

            //orderRepository.AddOrder(price, /*amount*/, wishCode, itemId, itemId2);   // Ben ermee bezig maar ga even commiten =D

            return RedirectToAction("Index", "Home");
        }
    }
}