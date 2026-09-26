using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics
{
    public class InventoryClerkInventoryStatisticsDTO
    {
        public int TotalInventoryItems { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int TotalQuantity { get; set; }
        public DashboardDistributionDTO? StockStatus { get; set; }
        public DashboardChartDTO? InventoryOverTime { get; set; }
    }
}
