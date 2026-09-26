using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs
{
    internal class InventoryClerkDashboardAggregateDTO
    {
        public InventoryClerkInventoryStatisticsDTO? Inventory { get; set; }
        public InventoryClerkSupplierStatisticsDTO? Supplier { get; set; }
        public InventoryClerkImportStatisticsDTO? Import { get; set; }
    }
}
