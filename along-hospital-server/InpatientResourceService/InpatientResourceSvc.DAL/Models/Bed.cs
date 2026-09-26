using InpatientResourceSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace InpatientResourceSvc.DAL.Models
{
    public class Bed : AuditEntity
    {
        [Unique]
        [MessageRequired]
        [MessageMaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [MessageRequired]
        public BedStatusEnum Status { get; set; } = BedStatusEnum.Active;

        [MessageRequired]
        public int RoomId { get; set; }

        [MessageRequired]
        public int BedCategoryId { get; set; }

        public virtual Room? Room { get; set; }
        public virtual BedCategory? BedCategory { get; set; }
        public virtual ICollection<BedOccupancy> BedOccupancies { get; set; } = [];
    }
}