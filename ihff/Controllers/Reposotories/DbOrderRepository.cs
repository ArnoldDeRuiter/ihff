using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    public class DbOrderRepository : IOrderRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();

        //Een order toevoegen aan de database.
        public void AddOrder(float totalPrice, int amount, string wishlistCode, int itemId)
        {
            Order order = new Order(totalPrice, amount, wishlistCode, itemId);

            ctx.Orderlines.Add(order);
            ctx.SaveChanges();
        }
    }
}