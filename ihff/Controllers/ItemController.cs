using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class ItemController : BaseController
    {
        private IItemRepository itemRepository = new DbItemRepository();
        public ActionResult Index()
        {
            // Nothing to see here people.. Go back to home page
            // Mocht men ooit op item/index belanden
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Detail(int itemId)
        {
            // Haal item op en stuur item naar de detail view
            Item item = itemRepository.GetItem(itemId);
            return View(item);
        }
    }
}