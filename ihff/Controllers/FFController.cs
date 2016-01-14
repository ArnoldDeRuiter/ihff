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
        // GET: FF
        public ActionResult Index(int Id)
        {
            Item item = itemRepository.GetItem(Id);
            IEnumerable<Item> diners = itemRepository.GetDinerDay(item.DateBegin);

            return View(diners);
        }
    }
}