using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class OrderItemCombined
    {
        public class Order
        {
            public double? TotalPrice { get; set; }
            public int Amount { get; set; }
            public int ItemId { get; set; }
            public int? ItemId2 { get; set; }
            public string WishlistCode { get; set; }

        }
        public class Item
        {
            public int ItemId { get; set; }
            public string Name { get; set; }
            public string DescriptionNL { get; set; }
            public string DescriptionENG { get; set; }
            public string Location { get; set; }
            public double? Price { get; set; }
            public DateTime DateBegin { get; set; }
            public DateTime DateEnd { get; set; }
            public string Image { get; set; }
            public int EventType { get; set; }
        }
    }
}