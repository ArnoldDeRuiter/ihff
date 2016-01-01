using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Order
    {
        [Key]
        public int OrderId {get; set;}

        public DateTime Date { get; set; }
        public double TotalPrice {get; set;} 

    }
}