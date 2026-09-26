using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs
{
    public class GetWorkScheduleAssignmentDTO : MapFrom<WorkScheduleAssignment>
    {
        public int Id { get; set; }

        public string? LocationType { get; set; }

        public int WorkScheduleId { get; set; }

        public int StaffId { get; set; }

        public int LocationId { get; set; }

        public List<GetWorkSegmentDTO> WorkSegments { get; set; } = [];

        public GetStaffDTO? Staff { get; set; }

        public GetRoomDTO? Room { get; set; }

        public GetTeleRoomDTO? TeleRoom { get; set; }
    }
}