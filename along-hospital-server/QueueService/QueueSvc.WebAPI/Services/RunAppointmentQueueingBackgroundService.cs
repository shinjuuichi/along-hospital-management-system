using QueueSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace QueueSvc.WebAPI.Services
{
    public class RunAppointmentQueueingBackgroundService(IServiceProvider serviceProvider) : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var queueCommandService = this.GetService<IQueueCommandService>(scope);
            await queueCommandService.RunAppointmentQueueingBackgroundAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}