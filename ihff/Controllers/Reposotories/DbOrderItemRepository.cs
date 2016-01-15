using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    public class DbOrderItemRepository : IOrderItemRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();
        public List<Order> GetOrders(string code)
        {
            List<Order> ordersPerCode = ctx.Orderlines.Where(o => o.WishlistCode == code).ToList();
            
            //foreach (Order o in ctx.Orderlines)
            //{

            //    if (o.WishlistCode == code)
            //    {
            //        ordersPerCode.Add(o);
            //    }
            //}
            return ordersPerCode;
        }

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
    }
}