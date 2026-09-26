using PatientSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PatientSvc.DAL.Models
{
    public class Allergy : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public SeverityLevelEnum SeverityLevel { get; set; } = SeverityLevelEnum.Mild;

        [MessageMaxLength(500)]
        public string? Reaction { get; set; }

        [MessageRequired]
        public int PatientId { get; set; }

        public virtual Patient? Patient { get; set; }
    }
}
