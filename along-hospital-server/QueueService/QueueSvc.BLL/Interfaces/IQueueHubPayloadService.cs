using QueueSvc.BLL.DTOs.QueueHubDTOs;
using QueueSvc.DAL.Enums;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueHubPayloadService
    {
        Task<QueueHubSnapshotDTO> GetQueueSnapshotAsync(int? roomId);

        Task<QueueHubSnapshotDTO> BuildQueueCreatedPayloadAsync(int queueNumber, int? roomId);

        Task<QueueHubSnapshotDTO> BuildListQueueCreatedPayloadAsync(int numberOfCreatedQueue);

        Task<QueueHubSnapshotDTO> BuildQueueUpdatedPayloadAsync(int queueNumber, int? roomId);

        Task<QueueHubSnapshotDTO> BuildQueueStatusUpdatedPayloadAsync(int queueNumber, QueueStatusEnum status, int? roomId);
    }
}
