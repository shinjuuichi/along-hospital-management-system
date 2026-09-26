using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GenerateWorkSegmentsEvent : BaseEvent
    {
        public DateTime PeriodStart { get; init; }

        public DateTime PeriodEnd { get; init; }

        public DateTime TriggeredAt { get; init; }
    }
}