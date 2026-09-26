using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs
{
    public class GetWorkScheduleAssignmentForSegmentDTO : MapFrom<WorkScheduleAssignment>
    {
        public int Id { get; set; }

        public int StaffId { get; set; }

        public int WorkScheduleId { get; set; }

        public int ShiftId { get; set; }

        public DateOnly WorkDate { get; set; }

        public TimeOnly ShiftStartTime { get; set; }

        public TimeOnly ShiftEndTime { get; set; }

        public bool IsOvertimeShift { get; set; }
    }
}
