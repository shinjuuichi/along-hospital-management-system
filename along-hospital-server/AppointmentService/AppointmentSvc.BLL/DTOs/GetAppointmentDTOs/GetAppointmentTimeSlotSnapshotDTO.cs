using AppointmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AppointmentSvc.BLL.DTOs.GetAppointmentDTOs
{
    public class GetAppointmentTimeSlotSnapshotDTO : MapFrom<TimeSlotSnapshot>
    {
        public int Id { get; set; }

        public TimeOnly Time { get; set; }
    }
}