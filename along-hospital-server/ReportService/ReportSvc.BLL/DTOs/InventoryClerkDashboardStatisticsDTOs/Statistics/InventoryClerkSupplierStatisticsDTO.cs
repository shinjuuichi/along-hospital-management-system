using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics
{
    public class InventoryClerkSupplierStatisticsDTO
    {
        public int TotalSuppliers { get; set; }
        public DashboardChartDTO? SuppliersOverTime { get; set; }
    }
}
