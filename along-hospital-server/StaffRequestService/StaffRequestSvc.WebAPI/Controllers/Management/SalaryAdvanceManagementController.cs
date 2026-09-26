using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = RolePolicies.SalaryAdvanceManagementRolePolicy)]
    public class SalaryAdvanceManagementController(ISalaryAdvanceService salaryAdvanceService)
        : GetController<GetSalaryAdvanceDTO, SalaryAdvanceFilterDTO>(salaryAdvanceService)
    {
        private readonly ISalaryAdvanceService _salaryAdvanceService = salaryAdvanceService;

        [Authorize(Roles = RolePolicies.SalaryAdvanceManagementRolePolicy)]
        [HttpPut("approve")]
        public async Task<IActionResult> ApproveSalaryAdvance([FromQuery] List<int> ids)
        {
            await _salaryAdvanceService.UpdateListStatusAsync(ids, SalaryAdvanceStatusEnum.Approved);
            return Result.SuccessAction("Salary advance approved successfully");
        }

        [Authorize(Roles = RolePolicies.SalaryAdvanceManagementRolePolicy)]
        [HttpPut("reject")]
        public async Task<IActionResult> RejectSalaryAdvance([FromQuery] List<int> ids)
        {
            await _salaryAdvanceService.UpdateListStatusAsync(ids, SalaryAdvanceStatusEnum.Rejected);
            return Result.SuccessAction("Salary advance rejected successfully");
        }
    }
}
