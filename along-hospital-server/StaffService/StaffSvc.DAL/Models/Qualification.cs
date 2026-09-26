using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace StaffSvc.DAL.Models
{
    public class Qualification : AuditEntity
    {
        [MessageRequired, MessageMaxLength(255), Unique]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; } = [];
    }
}