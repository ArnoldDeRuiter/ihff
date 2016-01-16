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
        private WishlistController wishController = new WishlistController();
        // GET: FF
        public ActionResult Index(int Id)
        {
            Item item = itemRepository.GetItem(Id);
            IEnumerable<Item> diners = itemRepository.GetDinerDay(item.DateBegin);

            ViewBag.item1 = item;

            return View(diners);
        }

        public ActionResult AddFFTicket(int itemId, int itemId2)
        {
            //double price = 67.99;
            //string wishCode = (string)Session["code"];

            wishController.addWish(itemId, itemId2);
            
            return RedirectToAction("Index", "Home");
        }
    }
}