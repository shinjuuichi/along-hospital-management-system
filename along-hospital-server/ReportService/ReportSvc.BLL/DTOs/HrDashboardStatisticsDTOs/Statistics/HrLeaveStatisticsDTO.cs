namespace ReportSvc.BLL.DTOs.HrDashboardStatisticsDTOs.Statistics
{
    public class HrLeaveStatisticsDTO
    {
        public int PendingLeaveRequests { get; set; }
        public int ApprovedLeaveRequests { get; set; }
        public int RejectedLeaveRequests { get; set; }
    }
}