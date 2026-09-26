using InpatientResourceSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace InpatientResourceSvc.DAL.Models
{
    public class Room : AuditEntity
    {
        [Unique]
        [MessageRequired]
        [MessageMaxLength(20)]
        public string Code { get; set; } = string.Empty;

        [MessageRequired]
        public RoomStatusEnum Status { get; set; } = RoomStatusEnum.Active;

        [MessageRequired]
        public int SpecialtyId { get; set; }

        [MessageRequired]
        public int FloorId { get; set; }

        [MessageRequired]
        public int RoomCategoryId { get; set; }

        public virtual Floor? Floor { get; set; }
        public virtual RoomCategory? RoomCategory { get; set; }
        public virtual ICollection<Bed> Beds { get; set; } = [];
    }
}
