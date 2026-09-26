using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces
{
    public interface IInventoryClerkDashboardService
    {
        Task<InventoryClerkDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
