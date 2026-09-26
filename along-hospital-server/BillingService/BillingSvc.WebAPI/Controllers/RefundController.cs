using BillingSvc.BLL.Interfaces;
using BillingSvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;

namespace BillingSvc.WebAPI.Controllers
{
    [Authorize]
    public class RefundController(IRefundService refundService) : BaseController
    {
        private readonly IRefundService _refundService = refundService;

        [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
        [HttpGet("invoice-charge/{clinicalMedicalOrderDetailId}")]
        public async Task<IActionResult> GetInvoiceIdAndChargeIdByClinicalMedicalOrderDetailId(string clinicalMedicalOrderDetailId)
        {
            var result = await _refundService.GetOrCreateInvoiceAndChargeIdByClinicalMedicalOrderDetailIdAsync(clinicalMedicalOrderDetailId);
            return Result.SuccessData(result);
        }

        [Authorize(Roles = nameof(RoleEnum.Accountant))]
        [HttpPut("approve/{chargeId}")]
        public async Task<IActionResult> Approve(int chargeId)
        {
            await _refundService.UpdateStatusByChargeIdAsync(chargeId, RefundStatusEnum.Approved);
            return Result.SuccessAction("Approve refund successfully.");
        }
    }
}