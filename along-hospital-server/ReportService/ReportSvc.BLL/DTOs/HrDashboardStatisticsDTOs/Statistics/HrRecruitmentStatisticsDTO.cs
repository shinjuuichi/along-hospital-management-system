namespace ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics
{
    public class HrRecruitmentStatisticsDTO
    {
        public int OpenJobPostings { get; set; }
        public int ApplicationsReceived { get; set; }
        public int PassedApplications { get; set; }
        public int FailedApplications { get; set; }
        public double InterviewPassRate { get; set; }
    }
}