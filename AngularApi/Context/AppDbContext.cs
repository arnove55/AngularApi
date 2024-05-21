using AngularApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace AngularApi.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }            

        public DbSet<User> Users { get; set; }
        //d
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<User>().ToTable("users");
            //d
           modelBuilder.Entity<Booking>().ToTable("Booking");

            modelBuilder.Entity<Coupon>().ToTable("Coupon");
        }
    }
}
