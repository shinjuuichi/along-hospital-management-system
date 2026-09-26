using ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs
{
    internal class HrDashboardAggregateDTO
    {
        public HrRecruitmentStatisticsDTO? Recruitment { get; set; }
        public HrLeaveStatisticsDTO? Leave { get; set; }
    }
}