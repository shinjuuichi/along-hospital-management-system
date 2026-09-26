using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace MedicalServiceSvc.DAL.Models
{
    public class MedicalService : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        [NumberPositive]
        public double Price { get; set; }

        public bool IsActive { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [MessageRequired]
        public int SpecialtyId { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<MedicalServiceRole> MedicalServiceRoles { get; set; } = [];
    }
}