using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts
{
    public record CreateUserToAuthContract : BaseContract
    {
        public int Id { get; init; }
    }
}
