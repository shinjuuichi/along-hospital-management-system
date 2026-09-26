using MessageBroker.Abstractions;

namespace MessageBroker.Events.AppointmentEvents
{
    public record GetAppointmentByTransactionIdEvent : BaseEvent
    {
        public Guid TransactionId { get; init; }
    }
}
