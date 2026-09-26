using MessageBroker.Contracts.AppointmentContracts;

namespace AppointmentSvc.BLL.Interfaces
{
    public interface IAppointmentStatisticsService
    {
        Task<GetAppointmentStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
