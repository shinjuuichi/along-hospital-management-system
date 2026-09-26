using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.Interfaces
{
    public interface IAccountantDashboardService
    {
        Task<AccountantDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
