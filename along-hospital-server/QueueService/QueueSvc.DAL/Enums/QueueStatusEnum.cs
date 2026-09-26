namespace QueueSvc.DAL.Enums
{
    public enum QueueStatusEnum
    {
        Waiting = 0,
        Called = 1,
        InProgress = 2,
        AwaitingResults = 3,
        Completed = 4,
        Cancelled = 5,
    }
}