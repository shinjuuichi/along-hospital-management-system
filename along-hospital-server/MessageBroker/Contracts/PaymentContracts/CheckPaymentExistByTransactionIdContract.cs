using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.PaymentContracts
{
    public record CheckPaymentExistByTransactionIdContract : BaseContract
    {
        public string PaymentUrl { get; init; } = string.Empty;
    }
}