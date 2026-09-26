using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.StaffRequestContracts
{
    public record GetStaffRequestStatisticsByDateRangeContract : BaseContract
    {
        public int PendingLeaveRequests { get; init; }
        public int ApprovedLeaveRequests { get; init; }
        public int RejectedLeaveRequests { get; init; }
        public int PendingSalaryAdvances { get; init; }
        public int ApprovedSalaryAdvances { get; init; }
        public int DisbursedSalaryAdvances { get; init; }
        public double SalaryAdvanceAmountApproved { get; init; }
        public StatisticsDistributionContract LeaveRequestStatus { get; init; } = new();
        public StatisticsDistributionContract SalaryAdvanceStatus { get; init; } = new();
    }
}
