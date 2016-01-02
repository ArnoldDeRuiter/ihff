using System;
using System.Collections.Generic;
using ihff.Models;

namespace ihff.Controllers
{
    public class InMemoryWishlistRepository : IWishlistRepository
    {
        private List<ihff.Models.Item> allItems = new List<ihff.Models.Item>();

        int itemId;//niet dat dit hier iets moet doen of zijn...
        
        public InMemoryWishlistRepository()
        {
            //DateTime dt = new DateTime();
            //Add some dummy items
            //allItems.Add(new ihff.Models.Item(0,"","","","",0.0,dt,dt,0.0,"","","",0,0,0,"",0));
        }

        public IEnumerable<ihff.Models.Item> GetAllItems()
        {
            return allItems;
        }
        
        public void Add(Item item)
        {
            allItems.Add(item);
        }
    }
}