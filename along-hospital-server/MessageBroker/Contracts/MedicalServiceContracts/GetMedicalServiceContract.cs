using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalServiceContracts
{
    public record GetMedicalServiceContract : BaseContract
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }

        public double Price { get; init; }

        public bool IsActive { get; init; }

        public string? Code { get; init; }

        public int SpecialtyId { get; init; }
    }

    public record GetListMedicalServiceDataContract : BaseContract
    {
        public List<GetMedicalServiceContract> Data { get; init; } = [];
    }
}