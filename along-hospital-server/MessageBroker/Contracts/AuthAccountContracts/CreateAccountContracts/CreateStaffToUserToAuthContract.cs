using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts
{
    public record CreateStaffToUserToAuthContract : BaseContract
    {
        public int Id { get; init; }
    }
}