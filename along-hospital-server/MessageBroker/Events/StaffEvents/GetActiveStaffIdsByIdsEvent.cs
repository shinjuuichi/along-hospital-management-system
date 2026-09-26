using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record GetActiveStaffIdsByIdsEvent : BaseEvent
    {
        public List<int> StaffIds { get; init; } = [];
    }
}