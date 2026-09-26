using MessageBroker.Abstractions;
using MessageBroker.Contracts.WorkScheduleContracts;

namespace MessageBroker.Events.StaffRequestEvents;

public record ValidateLeaveRequestEvent : BaseEvent
{
    public int StaffId { get; init; }
    public DateOnly FromDate { get; init; }
    public DateOnly ToDate { get; init; }
    public string? LeaveUnit { get; init; }
    public int? ShiftId { get; init; }
}

public record ValidateLeaveRequestContract : BaseContract;