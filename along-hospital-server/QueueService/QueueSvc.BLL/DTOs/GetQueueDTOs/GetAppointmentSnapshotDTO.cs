using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.GetQueueDTOs
{
    public class GetAppointmentSnapshotDTO : MapFrom<AppointmentSnapshot>
    {
        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public string? Purpose { get; set; }

        public string? AppointmentType { get; set; }

        public string? AppointmentMeetingType { get; set; }
    }
}