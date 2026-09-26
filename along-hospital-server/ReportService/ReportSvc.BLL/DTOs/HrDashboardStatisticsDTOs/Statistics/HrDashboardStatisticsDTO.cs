namespace ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics
{
    public class HrDashboardStatisticsDTO
    {
        public HrDashboardOverviewDTO? Overview { get; set; }
        public HrRecruitmentStatisticsDTO? Recruitment { get; set; }
        public HrLeaveStatisticsDTO? Leave { get; set; }
    }
}