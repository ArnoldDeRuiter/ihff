using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId {get; set;}

        public DateTime Date { get; set; }
        public double TotalPrice {get; set;} 

    }
}