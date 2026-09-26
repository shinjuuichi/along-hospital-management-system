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
    [Authorize(Roles = nameof(RoleEnum.HR))]
    [Route("api/v{version:apiVersion}/report/hr-dashboard")]
    public class HrDashboardController(
        IHrDashboardService hrDashboardService,
        IExcelService excelService) : BaseController
    {
        private readonly IHrDashboardService _hrDashboardService = hrDashboardService;
        private readonly IExcelService _excelService = excelService;

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics(DashboardDateRangeFilterDTO filterDTO)
        {
            var result = await _hrDashboardService.GetStatisticsAsync(filterDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportStatistics(DashboardDateRangeFilterDTO filterDTO)
        {
            var stats = await _hrDashboardService.GetStatisticsAsync(filterDTO);
            var title = "HR STATISTICS";
            var subtitle = $"From {filterDTO.FromDate:dd-MM-yyyy} To {filterDTO.ToDate:dd-MM-yyyy}";
            var excelBytes = await _excelService.WriteSectionedExcelAsync(stats, title, subtitle);
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"hr-statistics-{DateTime.UtcNow:yyyyMMdd}.xlsx");
        }
    }
}
