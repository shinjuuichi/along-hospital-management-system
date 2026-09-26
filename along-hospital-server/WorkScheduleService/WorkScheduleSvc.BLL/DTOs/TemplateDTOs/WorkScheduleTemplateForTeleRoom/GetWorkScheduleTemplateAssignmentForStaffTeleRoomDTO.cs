using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom
{
    public class GetWorkScheduleTemplateAssignmentForStaffTeleRoomDTO : MapFrom<WorkScheduleTemplateAssignmentForStaffTeleRoom>
    {
        public int WorkScheduleTemplateId { get; set; }

        public int ShiftId { get; set; }

        public int StaffId { get; set; }

        public int TeleRoomId { get; set; }

        public GetStaffDTO? Staff { get; set; }

        public GetTeleRoomDTO? TeleRoom { get; set; }
    }
}
