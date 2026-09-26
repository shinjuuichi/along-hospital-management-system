using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffRequestContracts
{
    public record ApprovedLeaveContract
    {
        public int StaffId { get; init; }

        public DateOnly FromDate { get; init; }

        public DateOnly ToDate { get; init; }

        public int? ShiftId { get; init; }

        public string? LeaveUnit { get; init; }
    }

    public record GetApprovedLeaveByStaffsAndRangeContract : BaseContract
    {
        public List<ApprovedLeaveContract> Data { get; init; } = [];
    }
}