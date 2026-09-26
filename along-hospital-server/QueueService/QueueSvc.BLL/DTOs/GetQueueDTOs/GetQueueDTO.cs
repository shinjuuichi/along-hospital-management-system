using QueueSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.GetQueueDTOs
{
    public class GetQueueDTO : MapFrom<Queue>
    {
        public int Id { get; set; }

        public DateTime QueueDate { get; set; }

        public int QueueNumber { get; set; }

        public int QueueOrder { get; set; }

        public string? QueueStatus { get; set; }

        public int NumberOfCalls { get; set; }

        public int Priority { get; set; }

        public bool IsReEntry { get; set; }

        public int? MedicalHistoryId { get; set; }

        public GetMedicalHistorySnapshotDTO? MedicalHistorySnapshot { get; set; }

        public int? SpecialtyId { get; set; }

        public GetSpecialtySnapshotDTO? SpecialtySnapshot { get; set; }

        public int? AppointmentId { get; set; }

        public GetAppointmentSnapshotDTO? AppointmentSnapshot { get; set; }

        public int? RoomId { get; set; }
    }
}
