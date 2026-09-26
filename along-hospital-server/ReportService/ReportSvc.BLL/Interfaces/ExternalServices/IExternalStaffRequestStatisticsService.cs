using MessageBroker.Contracts.StaffRequestContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalStaffRequestStatisticsService
    {
        Task<GetStaffRequestStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
