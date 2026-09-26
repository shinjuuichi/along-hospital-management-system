namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics
{
    public class InventoryClerkDashboardStatisticsDTO
    {
        public InventoryClerkDashboardOverviewDTO? Overview { get; set; }
        public InventoryClerkInventoryStatisticsDTO? Inventory { get; set; }
        public InventoryClerkSupplierStatisticsDTO? Supplier { get; set; }
        public InventoryClerkImportStatisticsDTO? Import { get; set; }
    }
}
