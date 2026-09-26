using MessageBroker.Contracts.MedicalHistoryContracts;

namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IMedicalHistoryStatisticsService
    {
        Task<GetMedicalHistoryStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
