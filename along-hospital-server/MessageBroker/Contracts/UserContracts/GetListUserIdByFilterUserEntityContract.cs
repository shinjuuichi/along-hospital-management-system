using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts
{
    public record GetListUserIdByFilterUserEntityContract : BaseContract
    {
        public List<int> UserIds { get; init; } = [];
    }
}