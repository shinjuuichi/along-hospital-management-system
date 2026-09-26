using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts
{
    public record GetPatientDataByUserIdContract : GetUserDataByUserIdContract
    {
        public string? MedicalNumber { get; init; }

        public int? Height { get; init; }

        public double? Weight { get; init; }

        public string? BloodType { get; init; }

        public List<GetPatientAllergyDataContractItem> Allergies { get; init; } = [];
    }

    public record GetPatientAllergyDataContractItem
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? SeverityLevel { get; init; }

        public string? Reaction { get; init; }
    }

    public record GetListPatientDataByUserIdsContract : BaseContract
    {
        public List<GetPatientDataByUserIdContract> Data { get; init; } = [];
    }
}