using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.UpdateAccountContracts
{
    public record UpdateAuthContract : BaseContract
    {
        public int UserId { get; init; }
    }
}
