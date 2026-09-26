using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace TeleHealthSvc.DAL.Models
{
    public class TeleRoom : BaseEntity
    {
        [Unique]
        [MessageRequired, MessageMaxLength(50)]
        public string RoomCode { get; set; } = string.Empty;

        [Unique]
        [MessageRequired, MessageMaxLength(100)]
        public string RoomDisplayName { get; set; } = string.Empty;

        [Unique]
        public int SpecialtyId { get; set; }

        public virtual ICollection<TeleSession> TeleSessions { get; set; } = [];
    }
}
