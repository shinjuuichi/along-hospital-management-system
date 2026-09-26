using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs
{
    public class GetWorkSegmentDTO : MapFrom<WorkSegment>
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? WorkStatus { get; set; }

        public string? WorkStatusReason { get; set; }

        public int WorkScheduleAssignmentId { get; set; }
    }
}
