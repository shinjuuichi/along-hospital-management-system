using Microsoft.AspNetCore.SignalR;
using QueueSvc.BLL.DTOs.QueueHubDTOs;
using QueueSvc.BLL.Interfaces;
using QueueSvc.BLL.Utils;
using QueueSvc.DAL.Enums;
using QueueSvc.WebAPI.Hubs;

namespace QueueSvc.WebAPI.Services
{
    public class QueueHubService(
        IHubContext<QueueHub> hubContext,
        IQueueHubPayloadService queueHubPayloadService) : IQueueHubService
    {
        private readonly IHubContext<QueueHub> _hubContext = hubContext;
        private readonly IQueueHubPayloadService _queueHubPayloadService = queueHubPayloadService;

        public Task<QueueHubSnapshotDTO> GetQueueSnapshotAsync(int? roomId)
        {
            return _queueHubPayloadService.GetQueueSnapshotAsync(roomId);
        }

        public async Task PublishQueueCreatedAsync(int queueNumber, int? roomId)
        {
            var normalizedRoomId = QueueHubChannelHelper.NormalizeRoomId(roomId);
            var payload = await _queueHubPayloadService.BuildQueueCreatedPayloadAsync(queueNumber, normalizedRoomId);

            await _hubContext.Clients
                .Group(QueueHubChannelHelper.GetRoomGroupName(normalizedRoomId))
                .SendAsync(QueueHubChannelHelper.RECEIVE_QUEUE_SNAPSHOT_EVENT, payload);
        }

        public async Task PublishListQueueCreatedAsync(int numberOfCreatedQueue)
        {
            var payload = await _queueHubPayloadService.BuildListQueueCreatedPayloadAsync(numberOfCreatedQueue);

            await _hubContext.Clients
                .Group(QueueHubChannelHelper.GetRoomGroupName(null))
                .SendAsync(QueueHubChannelHelper.RECEIVE_QUEUE_SNAPSHOT_EVENT, payload);
        }

        public async Task PublishQueueUpdatedAsync(int queueNumber, int? roomId)
        {
            var normalizedRoomId = QueueHubChannelHelper.NormalizeRoomId(roomId);
            var payloadTask = _queueHubPayloadService.BuildQueueUpdatedPayloadAsync(queueNumber, normalizedRoomId);

            await _hubContext.Clients
                .Group(QueueHubChannelHelper.GetRoomGroupName(normalizedRoomId))
                .SendAsync(QueueHubChannelHelper.RECEIVE_QUEUE_SNAPSHOT_EVENT, await payloadTask);
        }

        public async Task PublishQueueStatusUpdatedAsync(int queueNumber, QueueStatusEnum status, int? roomId)
        {
            var normalizedRoomId = QueueHubChannelHelper.NormalizeRoomId(roomId);
            var payload = await _queueHubPayloadService.BuildQueueStatusUpdatedPayloadAsync(queueNumber, status, normalizedRoomId);

            await _hubContext.Clients
                .Group(QueueHubChannelHelper.GetRoomGroupName(normalizedRoomId))
                .SendAsync(QueueHubChannelHelper.RECEIVE_QUEUE_SNAPSHOT_EVENT, payload);
        }
    }
}
