using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    public class DbOrderRepository : IOrderRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();
        private IItemRepository itemRepository = new DbItemRepository();

        public IEnumerable<Order> GetOrders(string code)
        {
            return ctx.Orderlines.Where(o => o.WishlistCode == code);

            //weggehaald en bovenstaande voor gebruikt.
 /*           List<Order> ordersPerCode = new List<Order>();

            foreach (Order o in ctx.Orderlines)
            {
                if (o.WishlistCode == code)
                {
                    ordersPerCode.Add(o);
                }
            }
            return ordersPerCode; */
        }

        //Een order toevoegen aan de database.
        public void AddOrder(double? totalPrice, int amount, string wishlistCode, int itemId, int? itemId2)
        {
            Order order = new Order();
            order.TotalPrice = totalPrice;
            order.Amount = amount;
            order.WishlistCode = wishlistCode;
            order.ItemId = itemId;
            order.ItemId2 = itemId2;

            ctx.Orderlines.Add(order);
            ctx.SaveChanges();
        }

        public bool checkAvailability(int amount, int itemId)
        {
            //todo maxAvailability hernoemen naar availability? uiteraard moet dan het field wel in mindering gebracht worden na het betalen van de order!
            Item item = itemRepository.GetItem(itemId);

            if (amount > item.MaxAvailabillity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}