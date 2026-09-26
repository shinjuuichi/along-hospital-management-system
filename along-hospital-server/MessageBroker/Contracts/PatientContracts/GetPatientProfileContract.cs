using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.PatientContracts
{
    public record GetPatientProfileContract : BaseContract
    {
        public string? MedicalNumber { get; init; }

        public int? Height { get; init; }

        public double? Weight { get; init; }

        public string? BloodType { get; init; }
    }
}
