using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom
{
    public class GetWorkScheduleTemplateAssignmentForStaffRoomDTO : MapFrom<WorkScheduleTemplateAssignmentForStaffRoom>
    {
        public int WorkScheduleTemplateId { get; set; }

        public int ShiftId { get; set; }

        public int StaffId { get; set; }

        public int RoomId { get; set; }

        public GetStaffDTO? Staff { get; set; }

        public GetRoomDTO? Room { get; set; }
    }
}
