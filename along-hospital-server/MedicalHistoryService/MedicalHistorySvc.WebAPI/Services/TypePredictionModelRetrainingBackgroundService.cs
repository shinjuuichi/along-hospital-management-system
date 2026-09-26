
using MedicalHistorySvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace MedicalHistorySvc.WebAPI.Services
{
    public class TypePredictionModelRetrainingBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var complaintService = this.GetService<IComplaintService>(scope);
            await complaintService.RetrainTypePredictionModelAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(30);
        }
    }
}