using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class EventItem
    {
        //[key]
        public int amount /*{get; set;}*/; 
        public string title /*{get; set;}*/; 
        public string description /*{get; set;}*/; 

        public DateTime datetime /*{get; set;}*/; 
        public Location location /*{get; set;}*/; 
    }
}