namespace ChatboxSvc.WebAPI.Interfaces
{
    public interface IAnalyticService
    {
        Task<string> SummarizeWeeklyComplaintsAsync(List<string> complaints);
    }
}
