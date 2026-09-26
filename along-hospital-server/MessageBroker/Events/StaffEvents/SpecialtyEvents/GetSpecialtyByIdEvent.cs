using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents.SpecialtyEvents
{
    public record GetSpecialtyByIdEvent : BaseEvent
    {
        public int SpecialtyId { get; init; }
    }

    public record GetListSpecialtyDataByIdsEvent : BaseEvent
    {
        public List<int> SpecialtyIds { get; init; } = [];
    }
}