using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Models;

namespace ihff.Models
{
    public class OrderItemCombined
    {
        public List<Order> Order { get; set; }
        public List<Item> Item { get; set; }
        public List<OrderItemCombined> AllCombined { get; set; }

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
        public int? Year { get; set; }
        public string Director { get; set; }
        public string Length { get; set; }
        public string DescriptionNL { get; set; }
        public string DescriptionENG { get; set; }
        public string Cast { get; set; }
        public int AgeClassification { get; set; }
    }
}