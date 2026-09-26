using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace TeleHealthSvc.DAL.Models
{
    public class TeleSession : AuditEntity
    {
        [MessageRequired]
        public string CredentialMetadataJson { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int AppointmentId { get; set; }

        public int TeleRoomId { get; set; }

        public int PatientId { get; set; }
    }
}