using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    interface IOrderRepository
    {
        void AddOrder(double totalPrice, int amount, string wishlistCode, int itemId, int? itemId2);

        bool checkAvailability(int amount, int itemId);

        IEnumerable<Order> GetOrders(string code);
    }
}