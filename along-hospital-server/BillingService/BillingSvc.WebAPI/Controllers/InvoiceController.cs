using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.FilterDTOs;
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
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class InvoiceController(IInvoiceService invoiceService)
        : GetController<GetInvoiceDTO, InvoiceFilterDTO>(invoiceService)
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        [AllowAnonymous]
        [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [HttpGet("{id}/payment-url")]
        public async Task<IActionResult> GetPaymentUrl(int id)
        {
            var paymentUrl = await _invoiceService.GetPaymentUrlByInvoiceIdAsync(id);
            return Result.SuccessData(paymentUrl, "Get payment URL successfully.");
        }

        [HttpPut("complete/{invoiceId}")]
        public async Task<IActionResult> Complete(int invoiceId)
        {
            await _invoiceService.UpdateStatusAsync(invoiceId, InvoiceStatusEnum.Completed);
            return Result.SuccessAction("Mark invoice as completed successfully.");
        }

        [HttpPut("cancel/{invoiceId}")]
        public async Task<IActionResult> Cancel(int invoiceId)
        {
            await _invoiceService.UpdateStatusAsync(invoiceId, InvoiceStatusEnum.Cancelled);
            return Result.SuccessAction("Cancel invoice successfully.");
        }
    }
}