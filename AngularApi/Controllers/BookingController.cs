using AngularApi.Context;
using AngularApi.Models.DTO;
using AngularApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using static Azure.Core.HttpHeader;

namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public BookingController(AppDbContext appdbcontext)
        {
            _appDbContext = appdbcontext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBooking([FromBody] AddBookingDto addBookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check for overlapping bookings
            var existingBooking = await _appDbContext.Booking
                .Where(b => b.User.Username == username &&
                            b.End_Date >= addBookingDto.Start_Date &&
                            b.Start_Date <= addBookingDto.End_Date)
                .FirstOrDefaultAsync();

            if (existingBooking != null)
            {
                return BadRequest($"You already have a booking from {existingBooking.Start_Date.ToShortDateString()} to {existingBooking.End_Date.ToShortDateString()}.");
            }
            
            var booking = new Booking
            {
                Booking_Date = DateTime.Now,
                Booking_Type = addBookingDto.Booking_Type,
                Start_Date = addBookingDto.Start_Date,
                End_Date = addBookingDto.End_Date,
                User = user
            };

            _appDbContext.Booking.Add(booking);
            await _appDbContext.SaveChangesAsync();

            //await 
            List<Coupon> coupons = new List<Coupon>();
            for (DateTime date = addBookingDto.Start_Date; date <= addBookingDto.End_Date; date = date.AddDays(1))
            {
                var coupon = new Coupon
                {
                    CouponCode = GenerateRandomAlphaNumeric(16),
                    Booking = booking,
                    CouponExpiry = DateTime.Now.AddHours(1),
                    Start_date=date,
                    End_date=date,
                    User = user
                };
                _appDbContext.Coupon.Add(coupon);
            }

           
            await _appDbContext.SaveChangesAsync();
            
            return Ok(booking);
        }




        //confirm code date 16 before 9 pm 
        //// POST: api/Booking
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> AddBooking([FromBody] AddBookingDto addBookingDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var username = User.Identity.Name;

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return Unauthorized("User ID not found in token.");
        //    }

        //    var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var booking = new Booking
        //    {
        //        Booking_Date = DateTime.Now,
        //        Booking_Type = addBookingDto.Booking_Type,
        //        Start_Date = addBookingDto.Start_Date,
        //        End_Date = addBookingDto.End_Date,
        //        User = user
        //    };

        //    _appDbContext.Booking.Add(booking);
        //    await _appDbContext.SaveChangesAsync();

        //    return Ok(booking);
        //}

        //cancle booking

        //[HttpDelete("cancel")]
        //[Authorize]
        //public async Task<IActionResult> CancelBooking([FromBody] CancleBookingdto cancelBookingDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var username = User.Identity.Name;

        //    if (string.IsNullOrEmpty(username))
        //    {
        //        return Unauthorized("User ID not found in token.");
        //    }

        //    var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

        //    if (user == null)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var booking = await _appDbContext.Booking.FirstOrDefaultAsync(b => b.User.Username == username && b.Start_Date.Date == cancelBookingDto.Start_Date.Date);

        //    if (booking == null)
        //    {
        //        return NotFound("Booking not found for the given date.");
        //    }

        //    // Validate the cancellation conditions
        //    if (booking.Start_Date.Date == DateTime.Today && DateTime.Now.Hour >= 20)
        //    {
        //        return BadRequest("Cannot cancel today's booking after 8 PM.");
        //    }

        //    if (booking.Start_Date.Date <= DateTime.Today)
        //    {
        //        return BadRequest("Cannot cancel today's or past bookings.");
        //    }

        //    _appDbContext.Booking.Remove(booking);
        //    await _appDbContext.SaveChangesAsync();

        //    return Ok(new { message = $"Booking for {booking.Start_Date.ToShortDateString()} has been successfully cancelled." });
        //}
        [HttpDelete("{date}")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(DateTime date)
        {
            var username=User.Identity.Name;
            if(string.IsNullOrEmpty(username) )
            {
                return Unauthorized("User not found");
            }
            var user=await _appDbContext.Users.FirstOrDefaultAsync(u=>u.Username==username);
            if(user==null)
            {
                return BadRequest("User not found");
            }
            var booking=await _appDbContext.Booking.Where(b=>b.User==user && b.Start_Date<=date && b.End_Date>=date).FirstOrDefaultAsync();
            var coupon=await _appDbContext.Coupon.Where(c=>c.User==user && c.Booking==booking).FirstOrDefaultAsync();
            if(booking==null)
            {
                return BadRequest("Booking not found for selected date");
            }
            if(DateTime.Now.Date>date.Date && DateTime.Now.Hour >= 20)
            {
                return BadRequest("Cannot cancel booking on the same day after 8 PM.");
            }
            if (DateTime.Now.Date > date.Date)
            {
                return BadRequest("Cannot cancel past bookings.");
            }
            if (booking.Start_Date == date)
            {
                _appDbContext.Booking.Remove(booking);
                
                
                _appDbContext.Coupon.Remove(coupon);
            }
            else if(booking.Start_Date == date)
            {
                booking.Start_Date.AddDays(1);
            }
            else if (booking.End_Date == date)
            {
                booking.End_Date.AddDays(-1);
            }
            else
            {
                var newbooking=new Booking{
                    Booking_Date=booking.Booking_Date,
                    End_Date=booking.End_Date,
                    Booking_Type=booking.Booking_Type,
                    Start_Date=date.AddDays(1),
                    User=booking.User,

                };
                booking.End_Date = date.AddDays(-1);
                _appDbContext.Booking.Add(booking);
            }
            await _appDbContext.SaveChangesAsync();
            return Ok(new { message = "Booking canceled" });
        }
       

        private static string GenerateRandomAlphaNumeric(int length)
        {
            const string grapn = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result=new StringBuilder(length);
            for (int i = 0;i < length; i++)
            {
                result.Append(grapn[random.Next(grapn.Length)]);
            }
            return result.ToString();
        }


    }
}

