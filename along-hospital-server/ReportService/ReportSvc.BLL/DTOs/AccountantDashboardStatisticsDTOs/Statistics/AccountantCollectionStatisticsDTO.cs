using ReportSvc.BLL.DTOs.DashboardSharedDTOs;

namespace ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics
{
    public class AccountantCollectionStatisticsDTO
    {
        public double CollectedAmount { get; set; }
        public double NetCollectedAmount { get; set; }
        public DashboardChartDTO? CollectionsOverTime { get; set; }
    }
}
