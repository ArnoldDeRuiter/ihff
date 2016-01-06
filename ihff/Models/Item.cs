using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    
    [Table("Items")]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string DescriptionNL { get; set; }
        public string DescriptionENG { get; set; }
        public string Location { get; set; }
        public double? Price { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public double? ImdbRating { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Length { get; set; }
        public int Year { get; set; }
        public int MaxAvailabillity { get; set; }
        public int AgeClassification { get; set; }
        public string Image { get; set; }
        public int EventType { get; set; }

        // old
        //public int amount /*{get; set;}*/; 
        //public string title /*{get; set;}*/; 
        //public string description /*{get; set;}*/; 

        //public DateTime datetime /*{get; set;}*/; 
        //public Location location /*{get; set;}*/; 
    }
}