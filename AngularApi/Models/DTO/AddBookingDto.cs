using System.ComponentModel.DataAnnotations;

namespace AngularApi.Models.DTO
{
    public class AddBookingDto
    {
        //d
        public string Booking_Type { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }

    }
}
