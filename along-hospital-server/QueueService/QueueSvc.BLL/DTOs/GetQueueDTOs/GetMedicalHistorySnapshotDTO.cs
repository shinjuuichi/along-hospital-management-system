using QueueSvc.BLL.DTOs.OtherDTOs;
using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.GetQueueDTOs
{
    public class GetMedicalHistorySnapshotDTO : MapFrom<MedicalHistorySnapshot>
    {
        public string? MedicalHistoryNumber { get; set; }

        public string? MedicalHistoryType { get; set; }

        public int PatientId { get; set; }

        public GetPatientDTO? Patient { get; set; }
    }
}