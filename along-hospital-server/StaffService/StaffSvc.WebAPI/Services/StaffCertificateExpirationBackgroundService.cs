using SharedLibrary.Base.Services;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Services
{
    public class StaffCertificateExpirationBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var certificateService = this.GetService<IStaffCertificateService>(scope);

            await certificateService.ExpireCertificatesAsync();
        }
    }
}