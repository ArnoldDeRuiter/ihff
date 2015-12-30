using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Models;

namespace ihff.Controllers
{
    public class DbItemRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();

        public IEnumerable<Item> GetAllItems()
        {
            IEnumerable<Item> allItems = ctx.Item;
            return allItems;
        }

        public IEnumerable<Item> GetAllMovies()
        {
            List<Item> allMovies = ctx.Item;
            foreach (Item item in allMovies)
            {
                if (item.EventType != 1)
                    allMovies.Remove(item);
            }

            return allMovies;
        }

        public IEnumerable<Item> GetAllSpecials()
        {
            List<Item> allSpecials = ctx.Item;
            foreach (Item item in allSpecials)
            {
                if (item.EventType != 2)
                    allSpecials.Remove(item);
            }

            return allSpecials;
        }

        public IEnumerable<Item> GetAllDiners()
        {
            List<Item> allDiners = ctx.Item;
            foreach (Item item in allDiners)
            {
                if (item.EventType != 3)
                    allDiners.Remove(item);
            }

            return allDiners;
        }
    }
}