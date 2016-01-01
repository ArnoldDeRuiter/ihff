using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Wishlist
    {
        [Key]
        public int WhislistId { get; set; }
        public List<Item> Item { get; set; }
    }
}