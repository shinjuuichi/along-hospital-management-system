using SharedLibrary.Base.Services;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Services
{
    public class VoucherExpirationBackgroundService(IServiceProvider _serviceProvider)
    : BaseBackgroundService(_serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            var now = DateTime.Now;
            var tomorrow = now.Date.AddDays(1);
            var timeUntilMidnight = tomorrow - now;

            return timeUntilMidnight.TotalMilliseconds > 0
                ? timeUntilMidnight
                : TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var voucherService = this.GetService<IVoucherService>(scope);
            await voucherService.ExpireVouchersAsync();
        }
    }
}
