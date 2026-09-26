using QueueSvc.BLL.DTOs.CreateQueueDTOs;
using QueueSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace QueueSvc.BLL.DTOs
{
    public class AssignMedicalHistoryToQueueDTO : MapTo<Queue>
    {
        public int MedicalHistoryId { get; set; }

        [JsonIgnore]
        public int SpecialtyId { get; set; }

        [JsonIgnore]
        public CreateMedicalHistorySnapshotDTO? MedicalHistorySnapshot { get; set; }

        [JsonIgnore]
        public CreateSpecialtySnapshotDTO? SpecialtySnapshot { get; set; }
    }
}
