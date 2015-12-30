using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Restaurant : Item
    {
        //[key]
        public string kitchentype /*{get; set;}*/; // Enumeratie maken? Nop halen we uit de db of is dit sneller ofzo (daar word gerwin blij van)? 
    }
}