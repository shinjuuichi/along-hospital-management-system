using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs
{
    public class GetPrescriptionDTO : MapFrom<Prescription>
    {
        public int Id { get; set; }

        public string? DoctorNote { get; set; }

        public int MedicationDays { get; set; }

        public int MedicalHistoryId { get; set; }

        public List<GetPrescriptionDetailDTO> PrescriptionDetails { get; set; } = [];
    }
}
