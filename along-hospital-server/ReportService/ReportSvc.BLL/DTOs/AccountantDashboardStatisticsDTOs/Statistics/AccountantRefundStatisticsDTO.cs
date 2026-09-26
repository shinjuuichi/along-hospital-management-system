using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics
{
    public class AccountantRefundStatisticsDTO
    {
        public double RefundAmount { get; set; }
        public int PendingRefunds { get; set; }
        public int ApprovedRefunds { get; set; }
        public int CancelledRefunds { get; set; }
        public DashboardDistributionDTO? RefundStatus { get; set; }
        public DashboardChartDTO? RefundsOverTime { get; set; }
    }
}
