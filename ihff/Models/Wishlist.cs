using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ihff.Models
{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public int WhishlistId { get; set; }
        public List<Item> Item { get; set; }
    }
}