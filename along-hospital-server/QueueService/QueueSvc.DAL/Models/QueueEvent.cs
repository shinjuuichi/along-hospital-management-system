using QueueSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;

namespace QueueSvc.DAL.Models
{
    public class QueueEvent : AuditEntity
    {
        public QueueStatusEnum FromStatus { get; set; } = QueueStatusEnum.Waiting;

        public QueueStatusEnum ToStatus { get; set; } = QueueStatusEnum.Waiting;

        public int QueueId { get; set; }

        public virtual Queue? Queue { get; set; }
    }
}