using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents;

public record SendStaffRequestStatusEmailEvent : BaseEvent
{
    public int StaffId { get; init; }
    public int? DeciderId { get; init; }
    public string RequestType { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string? Details { get; init; }
    public string? Reason { get; init; }
}
