using MessageBroker.Abstractions;

namespace MessageBroker.Events.AppointmentEvents
{
    public record GetListAppointmentDataByDateEvent : BaseEvent
    {
        public DateOnly Date { get; init; }
    }
}
