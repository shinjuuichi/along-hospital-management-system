using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces
{
    public interface IHrDashboardService
    {
        Task<HrDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}