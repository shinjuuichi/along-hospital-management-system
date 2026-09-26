namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics
{
    public class AccountantDashboardStatisticsDTO
    {
        public AccountantDashboardOverviewDTO? Overview { get; set; }
        public AccountantInvoiceStatisticsDTO? Invoice { get; set; }
        public AccountantCollectionStatisticsDTO? Collection { get; set; }
        public AccountantRefundStatisticsDTO? Refund { get; set; }
    }
}
