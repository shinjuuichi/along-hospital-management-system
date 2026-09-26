namespace ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs
{
    public class InventoryClerkDashboardOverviewDTO
    {
        public int TotalInventoryItems { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalImports { get; set; }
        public double TotalImportValue { get; set; }
    }
}
