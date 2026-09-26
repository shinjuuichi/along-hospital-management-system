using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.DAL.Models
{
    public class RoomCategoryRole : BaseEntity
    {
        [Unique]
        public RoleEnum Role { get; set; }

        public virtual ICollection<RoomCategoryRoleMapping> RoomCategoryRoleMappings { get; set; } = [];
    }
}
