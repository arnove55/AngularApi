using AngularApi.Context;
using AngularApi.Models;
using AngularApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CouponController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("{date}")]
        [Authorize]
       
        public async Task<IActionResult> GenerateCoupons(string date)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Use yyyy-MM-dd.");
            }

            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User not found");
            }


            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var booking = await _appDbContext.Booking.FirstOrDefaultAsync(b => b.User == user && b.Start_Date.Date == parsedDate.Date);
            if (booking == null)
            {
                return BadRequest("Booking not found for selected date.");
            }

            var coupons = GenerateCouponsForBooking(booking, user, parsedDate);
            _appDbContext.Coupon.AddRange(coupons);
            await _appDbContext.SaveChangesAsync();

            return Ok(coupons);
        }

        private IEnumerable<Coupon> GenerateCouponsForBooking(Booking booking, User user, DateTime date)
        {
            var coupons = new List<Coupon>();
            
                var coupon = new Coupon
                {
                    CouponCode = GenerateRandomAlphaNumeric(16),
                    Booking = booking,
                    CouponExpiry = date.AddMinutes(15), // Set expiry to end of the day
                    User = user,
                    Start_date = date,
                    End_date = date
                };
                coupons.Add(coupon);
            
            return coupons;
        }

        private static string GenerateRandomAlphaNumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }
    }
}
