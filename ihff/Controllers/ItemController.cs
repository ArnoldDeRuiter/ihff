using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class ItemController : Controller
    {
        private IItemRepository itemRepository = new DbItemRepository();
        // GET: Item
        public ActionResult Index()
        {
            IEnumerable<Item> allItems = itemRepository.GetAllMovies();
            return View(allItems.OrderBy(i => i.Name));
        }

        public ActionResult Detail(int itemId)
        {
            Item item = itemRepository.GetItem(itemId);
            return View(item);
        }
    }
}