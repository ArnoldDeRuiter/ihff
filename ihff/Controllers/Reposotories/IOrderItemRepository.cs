using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    interface IOrderItemRepository
    {
        List<Order> GetOrders(string code);
        Item GetItem(int itemId);
    }
}