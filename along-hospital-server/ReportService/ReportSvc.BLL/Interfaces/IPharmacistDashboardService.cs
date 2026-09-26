using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces
{
    public interface IPharmacistDashboardService
    {
        Task<PharmacistDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
