using SharedLibrary.Commons.EntityAnnotations;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom
{
    public class DeleteWorkScheduleTemplateAssignmentForStaffRoomDTO
    {
        public int ShiftId { get; set; }

        public int RoomId { get; set; }

        public int StaffId { get; set; }
    }
}
