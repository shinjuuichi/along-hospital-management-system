namespace AppointmentSvc.BLL.Interfaces
{
    public interface IAppointmentBackgroundService
    {
        Task CancelOverdueAppointmentsAsync();

        Task SendReminderEmailAsync();
    }
}
