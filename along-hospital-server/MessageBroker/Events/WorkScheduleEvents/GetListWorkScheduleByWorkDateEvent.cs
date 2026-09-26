using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetListWorkScheduleByWorkDateEvent : BaseEvent
    {
        public DateOnly WorkDate { get; init; }
    }
}