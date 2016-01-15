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

        public Item GetItem(int itemId)
        {
            Item itemsPerId = ctx.Items.Where(o => o.ItemId == itemId).Single<Item>();

            //foreach (Item i in ctx.Items)
            //{
            //    if (i.ItemId == itemId)
            //    {
            //        itemsPerId = i;
            //    }
            //}

            return itemsPerId;
        }

        public void AddOrder(Order order)
        {
            ctx.Orderlines.Add(order);
            ctx.SaveChanges();
        }
        

        public void RemoveOrder(int itemId)
        {
            var itemToRemove = ctx.Orderlines.SingleOrDefault(x => x.ItemId == 1); //returns a single item.
            ctx.Orderlines.Remove(itemToRemove);
            ctx.SaveChanges();
        }
    }
}