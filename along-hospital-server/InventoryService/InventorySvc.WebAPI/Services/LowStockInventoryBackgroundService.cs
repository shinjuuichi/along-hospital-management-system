using InventorySvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace InventorySvc.WebAPI.Services
{
    public class LowStockInventoryBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var inventoryService = this.GetService<IInventoryService>(scope);
            await inventoryService.SendLowStockAlertEmailsAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }
    }
}
