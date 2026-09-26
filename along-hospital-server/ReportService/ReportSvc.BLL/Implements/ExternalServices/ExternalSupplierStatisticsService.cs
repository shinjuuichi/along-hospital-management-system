using MessageBroker.Contracts.SupplierContracts;
using MessageBroker.Events.SupplierEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalSupplierStatisticsService(IMessageBus messageBus) : IExternalSupplierStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetSupplierStatisticsByDateRangeContract> GetSupplierStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetSupplierStatisticsByDateRangeEvent, GetSupplierStatisticsByDateRangeContract>(
                new GetSupplierStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }

        public async Task<GetImportStatisticsByDateRangeContract> GetImportStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetImportStatisticsByDateRangeEvent, GetImportStatisticsByDateRangeContract>(
                new GetImportStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
