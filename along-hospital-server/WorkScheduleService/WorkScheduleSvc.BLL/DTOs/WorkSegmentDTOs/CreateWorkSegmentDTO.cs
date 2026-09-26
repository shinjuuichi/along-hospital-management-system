using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs
{
    public class CreateWorkSegmentDTO : MapTo<WorkSegment>
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int WorkScheduleAssignmentId { get; set; }
    }
}