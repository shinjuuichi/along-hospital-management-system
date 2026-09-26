using MessageBroker.Abstractions;

namespace MessageBroker.Events.AppointmentEvents
{
    public record GetAppointmentStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
