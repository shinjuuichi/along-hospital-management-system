using AppointmentSvc.DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedLibrary.Base.Mappers;

namespace AppointmentSvc.BLL.DTOs
{
    public class CreateAppointmentRequestDTO : MapTo<Appointment>
    {
        public DateOnly Date { get; set; }

        public string? Purpose { get; set; }

        public string? AppointmentMeetingType { get; set; }

        public int SpecialtyId { get; set; }

        public int TimeSlotId { get; set; }

        [BindNever]
        public int PatientId { get; set; }
    }
}