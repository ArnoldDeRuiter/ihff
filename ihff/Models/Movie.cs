using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Movie : Item
    {
        //[key]
        public string ageclassification /*{get; set;}*/; // Enumeratie maken? Nop halen we uit de db of is dit sneller ofzo (daar word gerwin blij van)? 
        public string director /*{get; set;}*/; 
        public string starring /*{get; set;}*/; 
    }

}