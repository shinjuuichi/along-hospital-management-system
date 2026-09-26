using MessageBroker.Abstractions;

namespace MessageBroker.Events.AppointmentEvents
{
    public record UpdateListAppointmentStatusToCompletedEvent : BaseEvent
    {
        public List<int> Ids { get; set; } = [];
    }
}
