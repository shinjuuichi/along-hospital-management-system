using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics
{
    public class InventoryClerkImportStatisticsDTO
    {
        public int TotalImports { get; set; }
        public double TotalImportValue { get; set; }
        public DashboardChartDTO? ImportsOverTime { get; set; }
        public DashboardDistributionDTO? ImportBySupplier { get; set; }
    }
}
