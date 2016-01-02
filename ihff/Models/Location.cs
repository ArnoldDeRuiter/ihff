using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    [Table("Location")]
    public class Location
    {
        public int locationID { get; set;}
        [Key]
        public string Name {get; set;}
        public string address { get; set; }
        public string zipcode { get; set; }
        public int phonenumber { get; set; }
        public string omschrijvingNL { get; set; }
        public string omschrijvingEND { get; set; }
        public int capacity { get; set; }
    }
}