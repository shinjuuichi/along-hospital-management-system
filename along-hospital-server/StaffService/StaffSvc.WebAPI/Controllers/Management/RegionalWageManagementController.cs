using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.RegionalWageDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = $"{nameof(RoleEnum.Accountant)}, {nameof(RoleEnum.HR)}")]
    public class RegionalWageManagementController(IRegionalWageService regionalWageService)
        : GetController<GetRegionalWageDTO>(regionalWageService)
    {
        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRegionalWageDTO updateRegionalWageDTO)
        {
            var updatedRegionalWage = await regionalWageService.UpdateAsync(id, updateRegionalWageDTO);
            return Result.SuccessData(updatedRegionalWage, "Regional wage updated successfully.");
        }
    }
}