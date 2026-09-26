using SharedLibrary.Commons.EntityAbstractions;

namespace QueueSvc.DAL.Models.Snapshots
{
    public class AppointmentSnapshot : Entity
    {
        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public string? Purpose { get; set; }

        public string? AppointmentType { get; set; }

        public string? AppointmentMeetingType { get; set; }
    }
}