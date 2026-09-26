using QueueSvc.BLL.DTOs.QueueHubDTOs;
using QueueSvc.DAL.Enums;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueHubService
    {
        Task<QueueHubSnapshotDTO> GetQueueSnapshotAsync(int? roomId);

        Task PublishQueueCreatedAsync(int queueNumber, int? roomId);

        Task PublishListQueueCreatedAsync(int numberOfCreatedQueue);

        Task PublishQueueUpdatedAsync(int queueNumber, int? roomId);

        Task PublishQueueStatusUpdatedAsync(int queueNumber, QueueStatusEnum status, int? roomId);
    }
}
