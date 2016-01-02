using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ihff.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int ReservationId  {get; set;}
    }
}