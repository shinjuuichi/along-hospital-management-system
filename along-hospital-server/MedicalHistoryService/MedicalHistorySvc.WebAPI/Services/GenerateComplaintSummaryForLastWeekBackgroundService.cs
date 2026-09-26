using MedicalHistorySvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace MedicalHistorySvc.WebAPI.Services
{
    public class GenerateComplaintSummaryForLastWeekBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var complaintService = this.GetService<IComplaintService>(scope);
            await complaintService.CreateComplaintSummaryForLastWeekAsync();
        }
    }
}
