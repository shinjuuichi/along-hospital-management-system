using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record GetStaffProfileEvent : BaseEvent
    {
        public int StaffId { get; init; }
    }
}