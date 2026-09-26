using System.Text.Json.Serialization;

namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(GetMedicalHistoryClinicalMedicalOrderDTO))]
    [JsonDerivedType(typeof(GetMedicalHistoryInfusionMedicalOrderDTO))]
    [JsonDerivedType(typeof(GetMedicalHistoryInstructionMedicalOrderDTO))]
    public class GetMedicalHistoryMedicalOrderDTO
    {
        public string? Id { get; set; }

        public string? Instruction { get; set; }

        public string? MedicalOrderType { get; set; }

        public DateTime CreationDate { get; set; }

        public int MedicalHistoryId { get; set; }
    }
}
