using Microsoft.AspNetCore.SignalR;
using QueueSvc.BLL.Interfaces;
using QueueSvc.BLL.Utils;

namespace QueueSvc.WebAPI.Hubs
{
    public class QueueHub(IQueueHubService queueHubService) : Hub
    {
        private readonly IQueueHubService _queueHubService = queueHubService;

        public override async Task OnConnectedAsync()
        {
            var roomId = QueueHubChannelHelper.NormalizeRoomId(Context.GetHttpContext()?.Request.Query["roomId"].ToString());
            await this.JoinRoomGroupAsync(roomId);

            var queueSnapshot = await _queueHubService.GetQueueSnapshotAsync(roomId);
            await Clients.Caller.SendAsync(QueueHubChannelHelper.RECEIVE_QUEUE_SNAPSHOT_EVENT, queueSnapshot);

            await base.OnConnectedAsync();
        }

        private Task JoinRoomGroupAsync(int? roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, QueueHubChannelHelper.GetRoomGroupName(roomId));
        }
    }
}
