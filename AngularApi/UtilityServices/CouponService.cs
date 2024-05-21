
using AngularApi.Context;

namespace AngularApi.UtilityServices
{
    public class CouponService : IHostedService, IDisposable
    {
        
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        public CouponService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DeleteCoupon,null,TimeSpan.Zero,TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }
        private async void DeleteCoupon(object state)
        {
            using(var scope=_serviceProvider.CreateScope())
            {
                var context=scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var expiredCoupon=context.Coupon.Where(c=>c.CouponExpiry<=DateTime.Now).ToList();
                if (expiredCoupon.Any())
                {
                    context.Coupon.RemoveRange(expiredCoupon);
                    await context.SaveChangesAsync();
                }
            }
        }
       
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
