using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace StaffSvc.DAL.Models
{
    [PrimaryKey(nameof(StaffGroupId), nameof(StaffId))]
    public class StaffGroupMember : Entity
    {
        [MessageRequired]
        public int StaffGroupId { get; set; }

        [MessageRequired]
        public int StaffId { get; set; }

        public virtual StaffGroup? StaffGroup { get; set; }
        public virtual Staff? Staff { get; set; }
    }
}