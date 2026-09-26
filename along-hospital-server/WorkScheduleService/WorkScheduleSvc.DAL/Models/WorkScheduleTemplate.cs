using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace WorkScheduleSvc.DAL.Models
{
    public class WorkScheduleTemplate : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<WorkScheduleTemplateDayShift> WorkScheduleTemplateDayShifts { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<WorkScheduleTemplateAssignmentForStaffRoom> WorkScheduleTemplateAssignmentForStaffRooms { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<WorkScheduleTemplateAssignmentForStaffTeleRoom> WorkScheduleTemplateAssignmentForStaffTeleRooms { get; set; } = [];
    }
}
