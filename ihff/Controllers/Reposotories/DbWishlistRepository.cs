using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class DbWishlistRepository : IWishlistRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();
        
        public IEnumerable<Item> GetAllItems()
        {
            List<Item> allItems = new List<Item>();
            foreach (Item item in ctx.Items)
            {
                allItems.Add(item);
            }
            return allItems;
        }

        public Item GetItem(int itemId)
        {
            return ctx.Items.SingleOrDefault(c => c.ItemId == itemId);
        }
    }
}