using MessageBroker.Abstractions;

namespace MessageBroker.Events.PaymentEvents
{
    public record CheckPaymentExistByTransactionIdEvent : BaseEvent
    {
        public Guid? TransactionId { get; init; }
    }
}