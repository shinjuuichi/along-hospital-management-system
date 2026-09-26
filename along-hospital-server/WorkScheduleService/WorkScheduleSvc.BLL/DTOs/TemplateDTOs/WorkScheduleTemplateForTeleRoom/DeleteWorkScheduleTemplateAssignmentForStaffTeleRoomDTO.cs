using SharedLibrary.Commons.EntityAnnotations;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom
{
    public class DeleteWorkScheduleTemplateAssignmentForStaffTeleRoomDTO
    {
        public int ShiftId { get; set; }

        public int TeleRoomId { get; set; }

        public int StaffId { get; set; }
    }
}
