using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace OrderSvc.WebAPI.Services
{
    public class OverdueOrdersBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var orderService = this.GetService<IOrderService>(scope);
            await orderService.ProcessOverdueOrdersAsync();
        }
    }
}
