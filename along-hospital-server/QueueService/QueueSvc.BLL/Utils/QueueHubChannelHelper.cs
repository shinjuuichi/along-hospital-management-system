namespace QueueSvc.BLL.Utils
{
    public static class QueueHubChannelHelper
    {
        private const string GROUP_PREFIX = "queue-room";
        private const string UNASSIGNED_GROUP = "queue-room-unassigned";

        public const string RECEIVE_QUEUE_SNAPSHOT_EVENT = "ReceiveQueueSnapshot";

        public static int? NormalizeRoomId(int? roomId)
        {
            return roomId is > 0 ? roomId : null;
        }

        public static int? NormalizeRoomId(string? roomIdRaw)
        {
            if (int.TryParse(roomIdRaw, out var roomId))
            {
                return NormalizeRoomId(roomId);
            }

            return null;
        }

        public static string GetRoomGroupName(int? roomId)
        {
            var normalizedRoomId = NormalizeRoomId(roomId);
            return normalizedRoomId.HasValue ? $"{GROUP_PREFIX}-{normalizedRoomId.Value}" : UNASSIGNED_GROUP;
        }
    }
}
