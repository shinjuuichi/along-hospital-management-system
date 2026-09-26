using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.OrderContracts
{
    public record CreateOrderByCartDataContract : BaseContract
    {
        public string? PaymentUrl { get; init; }
    }
}
