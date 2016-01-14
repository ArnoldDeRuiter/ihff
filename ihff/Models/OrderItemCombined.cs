using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class OrderItemCombined
    {
        public Order Order { get; set; }
        public Item Item { get; set; }
    }
}