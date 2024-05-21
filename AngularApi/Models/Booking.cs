using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AngularApi.Models
{
    public class Booking
    {
        //d
        [Key]
        public int BookingID { get; set; }
        public DateTime Booking_Date { get; set; }
        public string Booking_Type { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }

        [ForeignKey("ID")]
        public User User { get; set; }


    }
}
