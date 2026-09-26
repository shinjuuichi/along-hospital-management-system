using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs
{
    public class CreateMedicalHistoryDTO : MapTo<MedicalHistory>
    {
        public int PatientId { get; set; }

        public string? MedicalHistoryType { get; set; }

        public int SpecialtyId { get; set; }

        public int AssignedDoctorId { get; set; }

        [JsonIgnore]
        public string? MedicalHistoryNumber { get; set; }

        [JsonIgnore]
        public bool IsCreatedFromAppointment { get; set; } = false;
    }
}
