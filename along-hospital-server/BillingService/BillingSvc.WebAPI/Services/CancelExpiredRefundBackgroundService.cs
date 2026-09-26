using BillingSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace BillingSvc.WebAPI.Services
{
    public class CancelExpiredRefundBackgroundService(
        IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var refundService = this.GetService<IRefundService>(scope);
            await refundService.CancelExpiredRefundsAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }
    }
}