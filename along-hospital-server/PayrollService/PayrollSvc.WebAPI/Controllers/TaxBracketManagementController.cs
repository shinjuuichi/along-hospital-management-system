using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSvc.BLL.DTOs.TaxBracketDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class TaxBracketManagementController(ITaxBracketService taxBracketService)
    : GetController<GetTaxBracketDTO>(taxBracketService)
    {
        private readonly ITaxBracketService _taxBracketService = taxBracketService;

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaxBracketDTO updateTaxBracketDTO)
        {
            var result = await _taxBracketService.UpdateAsync(id, updateTaxBracketDTO);
            return Result.SuccessData(result, $"Tax Bracket updated successfully");
        }
    }
}
