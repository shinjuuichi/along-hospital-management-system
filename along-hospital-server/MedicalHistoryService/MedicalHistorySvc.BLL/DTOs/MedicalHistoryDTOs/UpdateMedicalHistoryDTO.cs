using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs
{
    public class UpdateMedicalHistoryDTO : MapTo<MedicalHistory>
    {
        public string? Diagnosis { get; set; }

        public DateOnly? FollowUpAppointmentDate { get; set; }
    }
}
