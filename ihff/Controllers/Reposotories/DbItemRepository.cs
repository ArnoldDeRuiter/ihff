using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class DbItemRepository : IItemRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();

        public List<Item> GetItems(int itemId)
        {
            List<Item> itemsPerId = new List<Item>();

            foreach (Item i in ctx.Items)
            {
                if (i.ItemId == itemId)
                {
                    itemsPerId.Add(i);
                }
            }

            return itemsPerId;
        }
        public IEnumerable<Item> GetAllItems()
        {
            return ctx.Items;
        }

        public Item GetItem(int itemId)
        {
            return ctx.Items.SingleOrDefault(c => c.ItemId == itemId);
        }

        public IEnumerable<Item> GetAllMovies()
        {
            return ctx.Items.Where(i => i.EventType == 1);
        }

        public IEnumerable<Item> GetAllSpecials()
        {
            return ctx.Items.Where(i => i.EventType == 2);
        }

        public IEnumerable<Item> GetAllDiners()
        {
            return ctx.Items.Where(i => i.EventType == 3);
        }

        public Location GetItemLocation(int itemId)
        {
            Item item = GetItem(itemId);
            return ctx.Locations.FirstOrDefault(l => l.Name == item.Location);
        }

        public IEnumerable<Item> GetDinerDay(DateTime date)
        {
            return ctx.Items.Where(i => i.DateBegin.Day == date.Day && i.EventType == 3); 
        } 
    }
}