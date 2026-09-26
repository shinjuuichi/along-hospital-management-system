using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace InpatientResourceSvc.DAL.Models
{
    public class Building : AuditEntity
    {
        [Unique]
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(100)]
        public string Location { get; set; } = string.Empty;

        public virtual ICollection<Floor> Floors { get; set; } = [];
    }
}