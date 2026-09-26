using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace InpatientResourceSvc.DAL.Models
{
    [Index(nameof(BuildingId), nameof(FloorNumber), IsUnique = true)]
    public class Floor : AuditEntity
    {
        [MessageRequired]
        public int FloorNumber { get; set; }

        [MessageRequired]
        public int BuildingId { get; set; }

        public virtual Building? Building { get; set; }
        public virtual ICollection<Room> Rooms { get; set; } = [];
    }
}