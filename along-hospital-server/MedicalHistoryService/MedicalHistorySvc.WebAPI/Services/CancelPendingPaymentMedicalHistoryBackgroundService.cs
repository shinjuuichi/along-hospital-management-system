using MedicalHistorySvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace MedicalHistorySvc.WebAPI.Services
{
    public class CancelPendingPaymentMedicalHistoryBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var medicalHistoryCommandService = this.GetService<IMedicalHistoryCommandService>(scope);
            await medicalHistoryCommandService.CancelPendingPaymentMedicalHistoriesAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }
    }
}