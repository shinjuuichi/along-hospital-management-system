using MessageBroker.Contracts.StatisticsContracts;

namespace StaffRequestSvc.BLL.DTOs.StatisticsDTOs
{
    public class StaffRequestStatisticsDTO
    {
        public int PendingLeaveRequests { get; set; }
        public int ApprovedLeaveRequests { get; set; }
        public int RejectedLeaveRequests { get; set; }
        public int PendingSalaryAdvances { get; set; }
        public int ApprovedSalaryAdvances { get; set; }
        public int DisbursedSalaryAdvances { get; set; }
        public double SalaryAdvanceAmountApproved { get; set; }
        public StatisticsDistributionContract LeaveRequestStatus { get; set; } = new();
        public StatisticsDistributionContract SalaryAdvanceStatus { get; set; } = new();
    }
}
