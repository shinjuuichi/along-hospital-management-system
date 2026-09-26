using MessageBroker.Abstractions;

namespace MessageBroker.Events.TeleHealthEvents.TeleSessionEvents
{
    public record GetAppointmentByTeleSessionIdEvent : BaseEvent
    {
        public int TeleSessionId { get; init; }
    }
}