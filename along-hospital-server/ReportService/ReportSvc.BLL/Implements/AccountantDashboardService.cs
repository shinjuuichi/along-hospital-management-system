using AutoMapper;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.DTOs.AccountantDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.Interfaces.ExternalServices;

namespace ReportSvc.BLL.Implements
{
    public class AccountantDashboardService(
        IMapper mapper,
        IExternalBillingStatisticsService externalBillingStatisticsService)
        : IAccountantDashboardService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExternalBillingStatisticsService _externalBillingStatisticsService = externalBillingStatisticsService;

        public async Task<AccountantDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            var billing = await _externalBillingStatisticsService.GetStatisticsAsync(filterDTO);

            return _mapper.Map<AccountantDashboardStatisticsDTO>(new AccountantDashboardAggregateDTO
            {
                Invoice = _mapper.Map<AccountantInvoiceStatisticsDTO>(billing),
                Collection = _mapper.Map<AccountantCollectionStatisticsDTO>(billing),
                Refund = _mapper.Map<AccountantRefundStatisticsDTO>(billing)
            });
        }
    }
}
