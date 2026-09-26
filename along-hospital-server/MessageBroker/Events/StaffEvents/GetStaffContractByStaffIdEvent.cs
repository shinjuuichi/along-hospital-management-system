using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record GetStaffContractByStaffIdEvent : BaseEvent
    {
        public int StaffId { get; init; }
    }
}