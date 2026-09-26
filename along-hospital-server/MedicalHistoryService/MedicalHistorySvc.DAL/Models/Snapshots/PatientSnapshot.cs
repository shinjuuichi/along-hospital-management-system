using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalHistorySvc.DAL.Models.Snapshots
{
    public class PatientSnapshot : BaseEntity
    {
        // Auth Data
        public string? Phone { get; set; }

        public string? Email { get; set; }

        // User Data
        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        // Patient Data
        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public virtual ICollection<AllergySnapshot> Allergies { get; set; } = [];
    }
}
