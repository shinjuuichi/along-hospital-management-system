using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts
{
    public record GetListUserIdByFilterAuthAccountEntityContract : BaseContract
    {
        public List<int?> UserIds { get; init; } = [];
    }
}