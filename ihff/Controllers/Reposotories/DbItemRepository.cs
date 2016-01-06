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

        public IEnumerable<Item> GetAllMovies()
        {
            List<Item> allMovies = new List<Item>();
            
            foreach (Item item in ctx.Items.Where(item => item.EventType == 1))
            {
                allMovies.Add(item);
            }

            return allMovies;
        }

        public IEnumerable<Item> GetAllSpecials()
        {
            List<Item> allSpecials = new List<Item>();
            foreach (Item item in ctx.Items.Where(item => item.EventType == 2))
            {
                allSpecials.Add(item);
            }

            return allSpecials;
        }

        public IEnumerable<Item> GetAllDiners()
        {
            List<Item> allDiners = new List<Item>();
            foreach (Item item in ctx.Items.Where(item => item.EventType == 3))
            {
                allDiners.Add(item);
            }

            return allDiners;
        }
    }
}