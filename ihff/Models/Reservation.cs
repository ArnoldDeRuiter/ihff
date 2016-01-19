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

        [RegularExpression("^[a-zA-Z\\ ]+$")]
        [StringLength(50)]
        public string ReservationName { get; set; }

        [Required(ErrorMessage = "Your must provide a PhoneNumber")]
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string TelNumber { get; set; }
         
        [Required (ErrorMessage = "You must select a payment method")] 
        public string PaymentMethod { get; set; }
        public int PaymentSucces { get; set; }

        public Reservation()
        { }

        public Reservation(string reservationCode, string wishlistCode, string name)
        {
            this.ReservationCode = reservationCode;
            this.WishlistCode = wishlistCode;
            this.ReservationName = name;
        }
    }
}