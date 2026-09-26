using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace InpatientResourceSvc.DAL.Models
{
    public class RoomCategory : AuditEntity
    {
        [Unique]
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<RoomCategoryRoleMapping> RoomCategoryRoleMappings { get; set; } = [];
    }
}