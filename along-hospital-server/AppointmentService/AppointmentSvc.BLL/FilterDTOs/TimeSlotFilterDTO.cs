using AppointmentSvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace AppointmentSvc.BLL.FilterDTOs
{
    public class TimeSlotFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(TimeSlot.Time))]
        public TimeOnly? StartTime { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(TimeSlot.Time))]
        public TimeOnly? EndTime { get; set; }
    }
}