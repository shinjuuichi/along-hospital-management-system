using AppointmentSvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace AppointmentSvc.BLL.FilterDTOs
{
    public class AppointmentFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(Appointment.Date))]
        public DateOnly? StartDate { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(Appointment.Date))]
        public DateOnly? EndDate { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Appointment.AppointmentStatus))]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Appointment.AppointmentMeetingType))]
        public string? MeetingType { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? SpecialtyId { get; set; }

        public override int PageSize { get; set; } = 6;
    }
}