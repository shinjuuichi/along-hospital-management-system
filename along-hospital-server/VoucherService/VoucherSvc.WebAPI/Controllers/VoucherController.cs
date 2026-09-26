using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using VoucherSvc.BLL.DTOs.PatientVoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Controllers;

[Authorize(Roles = nameof(RoleEnum.Patient))]
public class VoucherController(
    IPatientVoucherService _patientVoucherService) : BaseController
{
    [AllowAnonymous]
    [HttpGet("collectible")]
    public async Task<IActionResult> GetCollectibleVouchers(VoucherFilterDTO filter)
    {
        var result = await _patientVoucherService.GetCollectibleVouchersAsync(filter);
        return Result.SuccessData(result);
    }

    [HttpPost("collect")]
    public async Task<IActionResult> CollectVoucher(CollectVoucherDTO request)
    {
        await _patientVoucherService.SelfCollectVoucherAsync(request);
        return Result.SuccessAction("Voucher collected successfully");
    }

    [HttpGet("my-vouchers")]
    public async Task<IActionResult> GetMyVouchers(PatientVoucherFilterDTO filter)
    {
        var result = await _patientVoucherService.GetMyVouchersAsync(filter);
        return Result.SuccessData(result);
    }

    [HttpGet("my-vouchers/all")]
    public async Task<IActionResult> GetAllMyVouchers()
    {
        var result = await _patientVoucherService.GetAllMyVouchersAsync();
        return Result.SuccessData(result);
    }
}
