using QueueSvc.DAL.Enums;
using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace QueueSvc.DAL.Models
{
    public class Queue : AuditEntity
    {
        public DateTime QueueDate { get; set; } = DateTime.UtcNow;

        [NumberHigherThanOrEqualTo(1)]
        public int QueueNumber { get; set; }

        [NumberHigherThanOrEqualTo(1)]
        public int QueueOrder { get; set; }

        public QueueStatusEnum QueueStatus { get; set; } = QueueStatusEnum.Waiting;

        [NumberPositive]
        public int NumberOfCalls { get; set; } = 0;

        [NumberPositive]
        public int Priority { get; set; } = 0;

        public bool IsReEntry { get; set; } = false;

        public int? MedicalHistoryId { get; set; }

        public int? SpecialtyId { get; set; }

        public int? AppointmentId { get; set; }

        public int? RoomId { get; set; }

        [JsonColumn]
        public virtual AppointmentSnapshot? AppointmentSnapshot { get; set; }

        [JsonColumn]
        public virtual MedicalHistorySnapshot? MedicalHistorySnapshot { get; set; }

        [JsonColumn]
        public virtual SpecialtySnapshot? SpecialtySnapshot { get; set; }

        public virtual ICollection<QueueEvent> QueueEvents { get; set; } = [];
    }
}
