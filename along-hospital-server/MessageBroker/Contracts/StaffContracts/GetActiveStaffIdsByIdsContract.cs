using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffContracts
{
    public record GetActiveStaffIdsByIdsContract : BaseContract
    {
        public List<int> StaffIds { get; init; } = [];
    }
}