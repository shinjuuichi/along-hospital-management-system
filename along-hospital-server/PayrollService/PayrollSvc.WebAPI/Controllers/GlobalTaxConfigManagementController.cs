using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSvc.BLL.DTOs.GlobalTaxConfigDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class GlobalTaxConfigManagementController(IGlobalTaxConfigService globalTaxConfigService)
    : GetController<GetGlobalTaxConfigDTO>(globalTaxConfigService)
    {
        private readonly IGlobalTaxConfigService _globalTaxConfigService = globalTaxConfigService;

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGlobalTaxConfigDTO updateGlobalTaxConfigDTO)
        {
            var result = await _globalTaxConfigService.UpdateAsync(id, updateGlobalTaxConfigDTO);
            return Result.SuccessData(result, $"Global Tax Config updated successfully");
        }
    }
}
