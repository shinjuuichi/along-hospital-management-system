using SharedLibrary.Commons.EntityAnnotations;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom
{
    public class CreateWorkScheduleTemplateAssignmentForStaffTeleRoomDTO
    {
        public int ShiftId { get; set; }

        public int TeleRoomId { get; set; }

        public List<int> StaffIds { get; set; } = [];
    }
}
