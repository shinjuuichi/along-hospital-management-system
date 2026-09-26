using MessageBroker.Abstractions;

namespace MessageBroker.Events.TeleHealthEvents.TeleSessionEvents
{
    public record GetTeleSessionByAppointmentIdEventItem
    {
        public int AppointmentId { get; init; }

        public Guid? TransactionId { get; init; }
    }

    public record GetTeleSessionByAppointmentIdsEvent : BaseEvent
    {
        public List<GetTeleSessionByAppointmentIdEventItem> Appointments { get; init; } = [];
    }
}
