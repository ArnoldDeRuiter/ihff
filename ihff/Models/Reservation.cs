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
        public string ReservationCode { get; set; }
        public string WishlistCode { get; set; }
        public string Name { get; set; }

        public Reservation()
        { }

        public Reservation(string reservationCode, string wishlistCode, string name)
        {
            this.ReservationCode = reservationCode;
            this.WishlistCode = wishlistCode;
            this.Name = name;
        }
    }
}