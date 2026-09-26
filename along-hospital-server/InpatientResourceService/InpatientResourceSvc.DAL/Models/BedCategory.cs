using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace InpatientResourceSvc.DAL.Models
{
    public class BedCategory : AuditEntity
    {
        [Unique]
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Unique]
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(255)]
        public string? Description { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<Bed> Beds { get; set; } = [];
    }
}