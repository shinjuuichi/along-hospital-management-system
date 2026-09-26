using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics
{
    public class AccountantInvoiceStatisticsDTO
    {
        public int TotalInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int CompletedInvoices { get; set; }
        public double GrossInvoiceAmount { get; set; }
        public DashboardDistributionDTO? InvoiceStatus { get; set; }
        public DashboardChartDTO? InvoicesOverTime { get; set; }
    }
}
