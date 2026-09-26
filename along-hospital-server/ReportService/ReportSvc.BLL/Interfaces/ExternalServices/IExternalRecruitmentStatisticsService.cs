using MessageBroker.Contracts.RecruitmentContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalRecruitmentStatisticsService
    {
        Task<GetRecruitmentStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
