using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffContracts.SpecialtyContracts
{
    public record GetAllSpecialtiesContract : BaseContract
    {
        public List<GetAllSpecialtiesContractItem> Specialties { get; init; } = [];
    }

    public record GetAllSpecialtiesContractItem
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
    }
}
