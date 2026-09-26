namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs
{
    public class AccountantDashboardOverviewDTO
    {
        public int TotalInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int CompletedInvoices { get; set; }
        public double NetCollectedAmount { get; set; }
        public double RefundAmount { get; set; }
        public int PendingRefunds { get; set; }
    }
}
