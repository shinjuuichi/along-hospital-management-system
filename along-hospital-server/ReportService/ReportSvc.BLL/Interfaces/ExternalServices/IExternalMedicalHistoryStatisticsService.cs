using MessageBroker.Contracts.MedicalHistoryContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalMedicalHistoryStatisticsService
    {
        Task<GetMedicalHistoryStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
