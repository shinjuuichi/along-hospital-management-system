using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetWorkScheduleByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }
}