using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ihff.Models
{
    public class OrderWishlist
    {
        //Oh... wanneer plotseling Wouter bedenkt dat hij al eens eerder een List vanaf een View naar Controller heeft gejonast. Helemaal prachtig.
        [Key]
        public string WishlistCode { get; set; }
        public int WishlistId { get; set; }

    }
}