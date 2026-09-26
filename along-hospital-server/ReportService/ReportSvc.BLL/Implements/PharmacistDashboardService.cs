using AutoMapper;
using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.Interfaces.ExternalServices;

namespace ReportSvc.BLL.Implements
{
    public class PharmacistDashboardService(
        IMapper mapper,
        IExternalOrderStatisticsService externalOrderStatisticsService)
        : IPharmacistDashboardService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExternalOrderStatisticsService _externalOrderStatisticsService = externalOrderStatisticsService;

        public async Task<PharmacistDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            var orderTask = _externalOrderStatisticsService.GetStatisticsAsync(filterDTO);
            var topMedicinesTask = _externalOrderStatisticsService.GetTopSellingMedicinesAsync(filterDTO, topN: 5);

            await Task.WhenAll(orderTask, topMedicinesTask);

            return _mapper.Map<PharmacistDashboardStatisticsDTO>(new PharmacistDashboardAggregateDTO
            {
                Order = _mapper.Map<PharmacistOrderStatisticsDTO>(await orderTask),
                TopMedicines = _mapper.Map<PharmacistTopMedicineStatisticsDTO>(await topMedicinesTask)
            });
        }
    }
}
