using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.BLL.DTOs.QueueHubDTOs;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueQueryService
    {
        Task<List<QueueHubSnapshotDTO>> GetAllTodayWorkingQueueAsync();

        Task<List<GetQueueDTO>> GetAllByTodayRoomIdAsync(int? roomId);

        Task<bool> HasAnyInProgressQueueTodayAsync(int? roomId, bool isOnlineQueue);

        Task<(int maxNumber, int maxOrder)> GetQueueAggregatesTodayAsync(int? roomId);

        Task<int> GetLastQueueOrderByPriorityAsync(int? roomId, int priority);
    }
}
