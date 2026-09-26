namespace ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs
{
    public class HrDashboardOverviewDTO
    {
        public int OpenJobPostings { get; set; }
        public int ApplicationsReceived { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int ApprovedLeaveRequests { get; set; }
    }
}