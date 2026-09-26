using MedicalHistorySvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs
{
    public class GetPrescriptionDetailMedicineSnapshotDTO : MapFrom<PrescriptionDetailMedicineSnapshot>
    {
        public string? MedicineName { get; set; }

        public string? MedicineBrand { get; set; }

        public string? MedicineImage { get; set; }

        public string? MedicineUnit { get; set; }
    }
}