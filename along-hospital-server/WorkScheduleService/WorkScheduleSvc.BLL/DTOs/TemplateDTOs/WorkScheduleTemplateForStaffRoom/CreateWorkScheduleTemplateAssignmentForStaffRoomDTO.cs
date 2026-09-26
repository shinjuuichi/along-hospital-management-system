using SharedLibrary.Commons.EntityAnnotations;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom
{
    public class CreateWorkScheduleTemplateAssignmentForStaffRoomDTO
    {
        public int ShiftId { get; set; }

        public int RoomId { get; set; }

        public List<int> StaffIds { get; set; } = [];
    }
}
