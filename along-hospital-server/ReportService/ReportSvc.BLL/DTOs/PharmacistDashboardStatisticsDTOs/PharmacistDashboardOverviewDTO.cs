namespace ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs
{
    public class PharmacistDashboardOverviewDTO
    {
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public double Revenue { get; set; }
        public int TotalOrders { get; set; }
    }
}
