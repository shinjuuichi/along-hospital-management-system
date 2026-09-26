using SharedLibrary.Base.Services;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Services
{
    public class StaffCertificateExpirationReminderBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(7);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var certificateService = this.GetService<IStaffCertificateService>(scope);

            await certificateService.SendExpirationReminderEmailsAsync();
        }
    }
}
