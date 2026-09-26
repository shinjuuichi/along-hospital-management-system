using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalHistorySvc.DAL.Models.Snapshots
{
    public class AllergySnapshot : BaseEntity
    {
        public string? Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Reaction { get; set; }
    }
}
