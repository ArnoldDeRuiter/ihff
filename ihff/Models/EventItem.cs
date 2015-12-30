using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string DescriptionNL { get; set; }
        public string DescriptionENG { get; set; }
        public string Location { get; set; }
        public float Price { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public float ImdbRating { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Lenght { get; set; }
        public int year { get; set; }
        public int MaxAvailabillity { get; set; }
        public string Img { get; set; }
        public int EventType { get; set; }

        //// old
        //public int amount /*{get; set;}*/; 
        //public string title /*{get; set;}*/; 
        //public string description /*{get; set;}*/; 

        //public DateTime datetime /*{get; set;}*/; 
        //public Location location /*{get; set;}*/; 
    }
}