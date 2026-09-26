using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.Interfaces
{
    public interface IManagerDashboardService
    {
        Task<ManagerDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
