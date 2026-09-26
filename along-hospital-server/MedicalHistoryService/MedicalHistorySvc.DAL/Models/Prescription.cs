using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalHistorySvc.DAL.Models
{
    public class Prescription : BaseEntity
    {
        [MessageMaxLength(1000)]
        public string? DoctorNote { get; set; }

        [NumberHigherThanOrEqualTo(1)]
        public int MedicationDays { get; set; }

        [MessageRequired]
        public int MedicalHistoryId { get; set; }

        public virtual MedicalHistory? MedicalHistory { get; set; }
        public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = [];
    }
}