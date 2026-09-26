using AutoMapper;
using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs;
using ReportSvc.BLL.DTOs.InventoryClerkDashboardStatisticsDTOs.Statistics;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.Interfaces.ExternalServices;

namespace ReportSvc.BLL.Implements
{
    public class InventoryClerkDashboardService(
        IMapper mapper,
        IExternalInventoryStatisticsService externalInventoryStatisticsService,
        IExternalSupplierStatisticsService externalSupplierStatisticsService)
        : IInventoryClerkDashboardService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExternalInventoryStatisticsService _externalInventoryStatisticsService = externalInventoryStatisticsService;
        private readonly IExternalSupplierStatisticsService _externalSupplierStatisticsService = externalSupplierStatisticsService;

        public async Task<InventoryClerkDashboardStatisticsDTO> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            var inventoryTask = _externalInventoryStatisticsService.GetStatisticsAsync(filterDTO);
            var supplierTask = _externalSupplierStatisticsService.GetSupplierStatisticsAsync(filterDTO);
            var importTask = _externalSupplierStatisticsService.GetImportStatisticsAsync(filterDTO);

            await Task.WhenAll(inventoryTask, supplierTask, importTask);

            return _mapper.Map<InventoryClerkDashboardStatisticsDTO>(new InventoryClerkDashboardAggregateDTO
            {
                Inventory = _mapper.Map<InventoryClerkInventoryStatisticsDTO>(await inventoryTask),
                Supplier = _mapper.Map<InventoryClerkSupplierStatisticsDTO>(await supplierTask),
                Import = _mapper.Map<InventoryClerkImportStatisticsDTO>(await importTask)
            });
        }
    }
}
