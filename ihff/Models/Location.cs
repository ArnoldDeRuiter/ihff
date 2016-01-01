using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Location
    {
        [Key]
        public string Name {get; set;}
        public string Description {get; set;} 
    }
}