using MessageBroker.Contracts.RecruitmentContracts;

namespace RecruitmentSvc.BLL.Interfaces
{
    public interface IRecruitmentStatisticsService
    {
        Task<GetRecruitmentStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
