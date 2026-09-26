using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.ManagerDashboardStatisticsDTOs.Statistics
{
    public class ManagerOrderStatisticsDTO
    {
        public double Revenue { get; set; }
        public int Orders { get; set; }
        public DashboardChartDTO? RevenueOverTime { get; set; }
        public DashboardChartDTO? OrdersOverTime { get; set; }
        public DashboardDistributionDTO? OrderStatus { get; set; }
    }
}
