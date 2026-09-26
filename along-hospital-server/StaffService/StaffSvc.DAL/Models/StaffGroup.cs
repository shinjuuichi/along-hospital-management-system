using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace StaffSvc.DAL.Models
{
    public class StaffGroup : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string Name { get; set; } = string.Empty;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<StaffGroupMember> StaffGroupMembers { get; set; } = [];
    }
}