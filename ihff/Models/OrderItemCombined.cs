using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ihff.Models
{
    public class OrderItemCombined
    {
        public class OrderItem
        {
            public virtual Order Order { get; set; }
            public virtual Item Item { get; set; }

            public double? TotalPrice { get; set; }
            public int Amount { get; set; }
            public int ItemId { get; set; }
            public int? ItemId2 { get; set; }
            public string WishlistCode { get; set; }
            //public int ItemId { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
            public double? Price { get; set; }
            public DateTime DateBegin { get; set; }
            public DateTime DateEnd { get; set; }
            public int MaxAvailabillity { get; set; }
            public string Image { get; set; }
            public int EventType { get; set; }
        }
    }
}