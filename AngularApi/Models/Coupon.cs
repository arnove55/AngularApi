using System.ComponentModel.DataAnnotations.Schema;

namespace AngularApi.Models
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string? CouponCode { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }

        public DateTime CouponExpiry { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}

