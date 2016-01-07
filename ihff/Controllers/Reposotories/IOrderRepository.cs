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
        void AddOrder(float totalPrice, int amount, string wishlistCode, int itemId);
    }
}