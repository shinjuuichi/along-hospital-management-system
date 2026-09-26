using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalServiceContracts
{
    public record GetAllMedicalServicesContract : BaseContract
    {
        public List<GetAllMedicalServicesContractItem> MedicalServices { get; init; } = [];
    }

    public record GetAllMedicalServicesContractItem
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public double Price { get; init; }
        public string? Code { get; init; }
    }
}