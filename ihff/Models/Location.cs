using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    [Table("Locations")]
    public class Location
    {
        public int locationId { get; set;}
        [Key]
        public string Name {get; set;}
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Phonenumber { get; set; }
        public string OmschrijvingNL { get; set; }
        public string OmschrijvingENG { get; set; }
        public int Capacity { get; set; }
    }
}