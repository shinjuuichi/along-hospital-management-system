using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.StaffRolePolicy)]
    public class SalaryAdvanceController(ISalaryAdvanceService salaryAdvanceService)
        : CrudController<CreateSalaryAdvanceDTO, UpdateSalaryAdvanceDTO, GetSalaryAdvanceDTO, SalaryAdvanceFilterDTO>(salaryAdvanceService)
    {
        protected override string? EntityName => "Salary advance";

        private readonly ISalaryAdvanceService _salaryAdvanceService = salaryAdvanceService;

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests(SalaryAdvanceFilterDTO filterDTO)
        {
            var result = await _salaryAdvanceService.GetMyRequestsAsync(filterDTO);
            return Result.SuccessData(result);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _salaryAdvanceService.UpdateStatusAsync(id, SalaryAdvanceStatusEnum.Cancelled);
            return Result.SuccessAction("Salary advance cancelled successfully");
        }
    }
}