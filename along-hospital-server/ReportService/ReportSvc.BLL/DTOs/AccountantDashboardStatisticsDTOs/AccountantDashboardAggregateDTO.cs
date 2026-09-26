using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs
{
    internal class AccountantDashboardAggregateDTO
    {
        public AccountantInvoiceStatisticsDTO? Invoice { get; set; }
        public AccountantCollectionStatisticsDTO? Collection { get; set; }
        public AccountantRefundStatisticsDTO? Refund { get; set; }
    }
}
