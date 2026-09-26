using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace WorkScheduleSvc.DAL.Models
{
    [PrimaryKey(nameof(WorkScheduleTemplateId), nameof(ShiftId), nameof(StaffId), nameof(RoomId))]
    public class WorkScheduleTemplateAssignmentForStaffRoom : Entity
    {
        #region Foreign Properties
        public int StaffId { get; set; }

        public int RoomId { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public virtual WorkScheduleTemplate? WorkScheduleTemplate { get; set; }
        public int WorkScheduleTemplateId { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public virtual Shift? Shift { get; set; }
        public int ShiftId { get; set; }
        #endregion
    }
}