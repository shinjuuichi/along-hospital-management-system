using SharedLibrary.Base.Services;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Services
{
    public class StaffContractExpiringNotificationBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval() => TimeSpan.FromDays(7);

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var staffContractService = this.GetService<IStaffContractService>(scope);

            await staffContractService.SendContractExpiringNotificationsAsync(30);
        }
    }
}
