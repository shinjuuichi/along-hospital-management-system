using MessageBroker.Contracts.StaffRequestContracts;

namespace StaffRequestSvc.BLL.Interfaces;

public interface IStaffRequestStatisticsService
{
    Task<GetStaffRequestStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
}
