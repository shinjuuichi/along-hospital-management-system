using QueueSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace QueueSvc.BLL.DTOs.CreateQueueDTOs
{
    public class CreateQueueFromQRDataDTO : MapTo<Queue>
    {
        public int MedicalHistoryId { get; set; }

        public int? RoomId { get; set; }

        [JsonIgnore]
        public int SpecialtyId { get; set; }

        [JsonIgnore]
        public CreateMedicalHistorySnapshotDTO? MedicalHistorySnapshot { get; set; }

        [JsonIgnore]
        public CreateSpecialtySnapshotDTO? SpecialtySnapshot { get; set; }
    }
}