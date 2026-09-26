using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalOrderEvents
{
    public record GetMedicalOrderStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
