using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts
{
    public record GetListUserIdByNameContainsAndRoleContract : BaseContract
    {
        public List<int> Ids { get; init; } = [];
    }
}