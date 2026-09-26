using MessageBroker.Abstractions;
using System.Text.Json.Serialization;

namespace MessageBroker.Contracts.MedicalOrderContracts
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(GetInfusionMedicalOrderContract), "Infusion")]
    [JsonDerivedType(typeof(GetInstructionMedicalOrderContract), "Instruction")]
    [JsonDerivedType(typeof(GetClinicalMedicalOrderContract), "Clinical")]
    public record GetMedicalOrderContract
    {
        public string? Id { get; init; }

        public string? Instruction { get; init; }

        public string? MedicalOrderType { get; init; }

        public DateTime CreationDate { get; init; }

        public int MedicalHistoryId { get; init; }
    }

    public record GetListMedicalOrderDataContract : BaseContract
    {
        public List<GetMedicalOrderContract> Data { get; init; } = [];
    }
}
