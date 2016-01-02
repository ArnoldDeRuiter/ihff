using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ihff.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId  {get; set;}
    }
}