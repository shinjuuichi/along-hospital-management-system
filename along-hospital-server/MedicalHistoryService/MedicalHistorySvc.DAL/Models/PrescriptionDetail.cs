using MedicalHistorySvc.DAL.Models.Snapshots;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalHistorySvc.DAL.Models
{
    [PrimaryKey(nameof(PrescriptionId), nameof(MedicineId))]
    public class PrescriptionDetail : Entity
    {
        public int PrescriptionId { get; set; }

        public int MedicineId { get; set; }

        [NumberPositive]
        public double Dosage { get; set; }

        [NumberPositive]
        public int FrequencyPerDay { get; set; }

        public virtual Prescription? Prescription { get; set; }

        [JsonColumn]
        public virtual PrescriptionDetailMedicineSnapshot? MedicineSnapshot { get; set; }
    }
}