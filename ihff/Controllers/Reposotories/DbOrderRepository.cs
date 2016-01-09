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

        //Een order toevoegen aan de database.
        public void AddOrder(float totalPrice, int amount, string wishlistCode, int itemId)
        {

            //Order order = new Order(totalPrice, amount, wishlistCode, itemId);
            //Sorry man, maar heb het voor je gefixed:
            Order order = new Order();
            order.TotalPrice = totalPrice;
            order.Amount = amount;
            order.WishlistCode = wishlistCode;
            order.ItemId = itemId;

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