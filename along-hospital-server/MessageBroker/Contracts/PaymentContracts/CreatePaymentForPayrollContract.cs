using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.PaymentContracts
{
    public record CreatePaymentForPayrollContract : BaseContract
    {
        public Guid TransactionId { get; init; }

        public string? PaymentUrl { get; init; }
    }
}