using MessageBroker.Contracts.AppointmentContracts;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueCacheService
    {
        Task<List<GetAppointmentContract>?> GetAppointmentsCacheAsync(DateOnly date);

        Task CreateAppointmentsCacheAsync(DateOnly date, List<GetAppointmentContract> appointmentCaches);

        Task UpdateAppointmentInCacheAsync(
            DateOnly date,
            int appointmentId,
            GetAppointmentContract appointment);
    }
}
