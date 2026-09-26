using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics
{
    public class PharmacistOrderStatisticsDTO
    {
        public double Revenue { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public DashboardChartDTO? OrdersOverTime { get; set; }
        public DashboardChartDTO? RevenueOverTime { get; set; }
        public DashboardDistributionDTO? OrderStatus { get; set; }
    }
}
