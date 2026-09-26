using InpatientResourceSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace InpatientResourceSvc.DAL.Models
{
    public class BedOccupancy : AuditEntity
    {
        public DateTime FromDateTime { get; set; } = DateTime.UtcNow;

        public DateTime? ToDateTime { get; set; }

        [MessageRequired]
        public OccupancyStatusEnum OccupancyStatus { get; set; } = OccupancyStatusEnum.Active;

        [MessageMaxLength(500)]
        public string? TransferNote { get; set; }

        [MessageRequired]
        public int MedicalHistoryId { get; set; }

        [MessageRequired]
        public int BedId { get; set; }

        public virtual Bed? Bed { get; set; }
    }
}