using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicalServiceContracts
{
    public record CreateMedicalServiceContract : BaseContract
    {
        public string? Name { get; init; }
        public string? Code { get; init; }
    }
}