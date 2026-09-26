using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using System.Text.Json.Serialization;

namespace MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(GetInfusionMedicalOrderDTO))]
    [JsonDerivedType(typeof(GetInstructionMedicalOrderDTO))]
    [JsonDerivedType(typeof(GetClinicalMedicalOrderDTO))]
    public class GetMedicalOrderDTO
    {
        public string? Id { get; set; }

        public string? Instruction { get; set; }

        public string? MedicalOrderType { get; set; }

        public DateTime CreationDate { get; set; }

        public int MedicalHistoryId { get; set; }
    }
}