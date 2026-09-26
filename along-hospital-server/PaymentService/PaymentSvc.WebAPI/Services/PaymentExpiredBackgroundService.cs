using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace PaymentSvc.WebAPI.Services
{
    public class PaymentExpiredBackgroundService(IServiceProvider serviceProvider) : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(7);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var paymentService = this.GetService<IPaymentService>(scope);
            await paymentService.UpdatePaymentExpiredAsync();
        }
    }
}