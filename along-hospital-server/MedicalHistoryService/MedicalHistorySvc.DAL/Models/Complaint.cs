using MedicalHistorySvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalHistorySvc.DAL.Models
{
    public class Complaint : AuditEntity
    {
        public ComplaintTopicEnum ComplaintTopic { get; set; } = ComplaintTopicEnum.Others;

        [MessageRequired]
        [MessageMaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Response { get; set; }

        public ComplaintTypeEnum ComplaintType { get; set; } = ComplaintTypeEnum.Neutral;

        public ComplaintResolveStatusEnum ComplaintResolveStatus { get; set; } = ComplaintResolveStatusEnum.Pending;

        [MessageRequired]
        public int MedicalHistoryId { get; set; }

        public virtual MedicalHistory? MedicalHistory { get; set; }
    }
}