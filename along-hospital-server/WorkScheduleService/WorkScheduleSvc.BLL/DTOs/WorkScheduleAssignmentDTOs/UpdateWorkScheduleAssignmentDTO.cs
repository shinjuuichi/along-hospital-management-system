using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs
{
    public class UpdateWorkScheduleAssignmentDTO : MapTo<WorkScheduleAssignment>
    {
        public string? LocationType { get; set; }

        public int StaffId { get; set; }

        public int LocationId { get; set; }
    }
}