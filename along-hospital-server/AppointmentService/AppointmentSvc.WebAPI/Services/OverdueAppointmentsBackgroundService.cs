using AppointmentSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace AppointmentSvc.WebAPI.Services
{
    public class OverdueAppointmentsBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var appointmentBackgroundService = this.GetService<IAppointmentBackgroundService>(scope);
            await appointmentBackgroundService.CancelOverdueAppointmentsAsync();
        }
    }
}