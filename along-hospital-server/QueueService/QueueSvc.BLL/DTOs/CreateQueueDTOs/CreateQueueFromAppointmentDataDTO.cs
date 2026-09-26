using QueueSvc.DAL.Models;
using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.CreateQueueDTOs
{
    public class CreateQueueFromAppointmentDataDTO : MapTo<Queue>
    {
        public int AppointmentId { get; set; }

        public int? MedicalHistoryId { get; set; }

        public int SpecialtyId { get; set; }

        public CreateAppointmentSnapshotDTO? AppointmentSnapshot { get; set; }

        public CreateMedicalHistorySnapshotDTO? MedicalHistorySnapshot { get; set; }

        public CreateSpecialtySnapshotDTO? SpecialtySnapshot { get; set; }

        public class CreateAppointmentSnapshotDTO : MapTo<AppointmentSnapshot>
        {
            public DateOnly Date { get; set; }

            public TimeOnly Time { get; set; }

            public string? Purpose { get; set; }

            public string? AppointmentType { get; set; }

            public string? AppointmentMeetingType { get; set; }
        }
    }
}