using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.CreateQueueDTOs
{
    public class CreateMedicalHistorySnapshotDTO : MapTo<MedicalHistorySnapshot>
    {
        public string? MedicalHistoryNumber { get; set; }

        public string? MedicalHistoryType { get; set; }

        public int PatientId { get; set; }
    }
}
