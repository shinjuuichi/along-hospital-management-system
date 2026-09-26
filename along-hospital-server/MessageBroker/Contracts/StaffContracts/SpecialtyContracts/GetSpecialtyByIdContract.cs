using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffContracts.SpecialtyContracts
{
    public record GetSpecialtyByIdContract : BaseContract
    {
        public int Id { get; init; }

        public string? Name { get; init; }
    }

    public record GetListSpecialtyDataByIdsContract : BaseContract
    {
        public List<GetSpecialtyByIdContract> Data { get; init; } = [];
    }
}