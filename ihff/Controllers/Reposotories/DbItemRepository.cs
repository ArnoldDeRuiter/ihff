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

        public IEnumerable<Item> GetAllItems()
        {
            IEnumerable<Item> allItems = ctx.Item;
            return allItems;
        }

        public Item GetItem(int itemId)
        {
            return ctx.Item.SingleOrDefault(c => c.ItemId == itemId);
        } 

        public IEnumerable<Item> GetAllMovies()
        {
            List<Item> allMovies = ctx.Item;
            foreach (Item item in allMovies.Where(item => item.EventType != 1))
            {
                allMovies.Remove(item);
            }

            return allMovies;
        }

        public IEnumerable<Item> GetAllSpecials()
        {
            List<Item> allSpecials = ctx.Item;
            foreach (Item item in allSpecials.Where(item => item.EventType != 2))
            {
                allSpecials.Remove(item);
            }

            return allSpecials;
        }

        public IEnumerable<Item> GetAllDiners()
        {
            List<Item> allDiners = ctx.Item;
            foreach (Item item in allDiners.Where(item => item.EventType != 3))
            {
                allDiners.Remove(item);
            }

            return allDiners;
        }
    }
}