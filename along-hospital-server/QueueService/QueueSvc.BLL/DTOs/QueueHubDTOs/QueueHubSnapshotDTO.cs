using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.BLL.DTOs.OtherDTOs;

namespace QueueSvc.BLL.DTOs.QueueHubDTOs
{
    public class QueueHubSnapshotDTO
    {
        public int? RoomId { get; set; }

        public GetRoomDTO? Room { get; set; }

        public int? DoctorId { get; set; }

        public GetStaffDTO? Doctor { get; set; }

        public List<GetQueueDTO> Queues { get; set; } = [];

        public QueueHubSummaryDTO Summary { get; set; } = new();

        public string? Message { get; set; }
    }
}
