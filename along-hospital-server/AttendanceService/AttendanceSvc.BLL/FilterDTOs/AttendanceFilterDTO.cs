using AttendanceSvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace AttendanceSvc.BLL.FilterDTOs
{
    public class AttendanceFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? LogType { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(Attendance.LogTime))]
        public DateTime? StartDate { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(Attendance.LogTime))]
        public DateTime? EndDate { get; set; }
    }
}
