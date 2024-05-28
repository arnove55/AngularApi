using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AngularApi.Context;
namespace AngularApi.UtilityServices
{
    public class CouponCleanupService:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public CouponCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested) 
            {
                await Task.Delay(TimeSpan.FromMinutes(15),stoppingToken);
                using(var scope = _scopeFactory.CreateScope())
                {

                    var dbcontext=scope.ServiceProvider.GetRequiredService<AppDbContext>(); ;
                    var expiredCoupons=await dbcontext.Coupon.Where(c=>c.CreatedDate.AddMinutes(15)<=DateTime.Now).ToListAsync();
                    if(expiredCoupons.Any() )
                    {
                        dbcontext.Coupon.RemoveRange(expiredCoupons);
                        await dbcontext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
