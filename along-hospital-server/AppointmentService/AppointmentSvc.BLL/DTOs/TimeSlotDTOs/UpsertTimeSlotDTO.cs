using AppointmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AppointmentSvc.BLL.DTOs.TimeSlotDTOs
{
    public class UpsertTimeSlotDTO : MapTo<TimeSlot>
    {
        public TimeOnly Time { get; set; }

        public int CapacityPerDoctor { get; set; }
    }
}
