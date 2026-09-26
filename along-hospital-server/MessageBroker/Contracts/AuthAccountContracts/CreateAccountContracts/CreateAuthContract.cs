using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts
{
    public record CreateAuthContract : BaseContract
    {
        public int UserId { get; init; }
    }
}
