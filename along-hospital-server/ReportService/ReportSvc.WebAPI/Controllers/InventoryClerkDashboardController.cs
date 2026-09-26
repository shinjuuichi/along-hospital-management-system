using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace ReportSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.InventoryClerk))]
    [Route("api/v{version:apiVersion}/report/inventory-clerk-dashboard")]
    public class InventoryClerkDashboardController(
        IInventoryClerkDashboardService inventoryClerkDashboardService,
        IExcelService excelService) : BaseController
    {
        private readonly IInventoryClerkDashboardService _inventoryClerkDashboardService = inventoryClerkDashboardService;
        private readonly IExcelService _excelService = excelService;

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics(DashboardDateRangeFilterDTO filterDTO)
        {
            var result = await _inventoryClerkDashboardService.GetStatisticsAsync(filterDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportStatistics(DashboardDateRangeFilterDTO filterDTO)
        {
            var stats = await _inventoryClerkDashboardService.GetStatisticsAsync(filterDTO);
            var title = "INVENTORY CLERK STATISTICS";
            var subtitle = $"From {filterDTO.FromDate:dd-MM-yyyy} To {filterDTO.ToDate:dd-MM-yyyy}";
            var excelBytes = await _excelService.WriteSectionedExcelAsync(stats, title, subtitle);
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"inventory-clerk-statistics-{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }
    }
}
