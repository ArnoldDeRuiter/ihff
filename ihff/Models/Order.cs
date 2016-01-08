using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    [Table("Orderslines")]
    public class Order
    {
        [Key]
        //public int OrderlineId {get; set;}
        public double? TotalPrice { get; set; }
        public int Amount { get; set; }
        public int ItemId { get; set; }
        public string WishlistCode { get; set; }

        //Sorry man
        /*public Order(double? totalPrice, int amount, string wishlistCode, int itemId)
        {
            this.TotalPrice = totalPrice;
            this.Amount = amount;
            this.WishlistCode = wishlistCode;
            this.ItemId = ItemId;
        }*/

    }
}