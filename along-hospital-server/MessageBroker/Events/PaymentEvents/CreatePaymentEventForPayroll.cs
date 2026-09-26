using MessageBroker.Abstractions;

namespace MessageBroker.Events.PaymentEvents
{
    public record CreatePaymentEventForPayroll : BaseEvent
    {
        public string? Description { get; init; }

        public string? BankCode { get; init; }

        public string? AccountNumber { get; init; }

        public List<PaymentEventItem> PaymentEventItems { get; init; } = [];
    }
}
