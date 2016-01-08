using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ihff.Models
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        public string WishlistCode { get; set; }
        public int WishlistId { get; set; }

    }
}