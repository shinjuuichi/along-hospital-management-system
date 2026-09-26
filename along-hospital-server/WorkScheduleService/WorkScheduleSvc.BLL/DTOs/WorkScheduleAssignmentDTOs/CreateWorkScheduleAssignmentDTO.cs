using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs
{
    public class CreateWorkScheduleAssignmentDTO : MapTo<WorkScheduleAssignment>
    {
        public string? LocationType { get; set; }

        public int WorkScheduleId { get; set; }

        public int LocationId { get; set; }

        public List<int> StaffIds { get; set; } = [];
    }
}