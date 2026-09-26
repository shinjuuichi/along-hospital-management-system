using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using VoucherSvc.BLL.DTOs.VoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Controllers;

[Authorize(Roles = (nameof(RoleEnum.Manager)))]
public class VoucherManagementController(
    IVoucherService _voucherManagementService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateVoucher(CreateVoucherDTO dto)
    {
        var result = await _voucherManagementService.CreateAsync(dto);
        return Result.SuccessData(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVoucher(string id, UpdateVoucherDTO dto)
    {
        var result = await _voucherManagementService.UpdateAsync(id, dto);
        return Result.SuccessData(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVoucher(string id)
    {
        await _voucherManagementService.DeleteAsync(id);
        return Result.SuccessAction("Voucher deleted successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVoucherById(string id)
    {
        var result = await _voucherManagementService.GetByIdAsync(id);
        return Result.SuccessData(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVouchers(VoucherFilterDTO filter)
    {
        var result = await _voucherManagementService.GetAllAsync(filter);
        return Result.SuccessData(result);
    }
}
