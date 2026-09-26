using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalHistorySvc.DAL.Models.Snapshots
{
    public class PrescriptionDetailMedicineSnapshot : Entity
    {
        public string? MedicineName { get; set; }

        public string? MedicineBrand { get; set; }

        public string? MedicineImage { get; set; }

        public string? MedicineUnit { get; set; }
    }
}