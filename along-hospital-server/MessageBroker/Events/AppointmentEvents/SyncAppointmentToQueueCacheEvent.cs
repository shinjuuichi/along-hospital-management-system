using MessageBroker.Abstractions;
using MessageBroker.Contracts.AppointmentContracts;

namespace MessageBroker.Events.AppointmentEvents
{
    public record SyncAppointmentToQueueCacheEvent : BaseEvent
    {
        public GetAppointmentContract? Appointment { get; init; }
    }
}
