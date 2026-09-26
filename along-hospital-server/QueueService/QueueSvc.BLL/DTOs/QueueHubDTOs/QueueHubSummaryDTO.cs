namespace QueueSvc.BLL.DTOs.QueueHubDTOs
{
    public class QueueHubSummaryDTO
    {
        public int Total { get; set; }

        public int Waiting { get; set; }

        public int Called { get; set; }

        public int InProgress { get; set; }

        public int AwaitingResults { get; set; }

        public int Completed { get; set; }

        public int Cancelled { get; set; }
    }
}
