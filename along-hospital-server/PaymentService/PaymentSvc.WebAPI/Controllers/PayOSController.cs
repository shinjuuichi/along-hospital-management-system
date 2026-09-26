using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using PaymentSvc.BLL.DTOs.PayOSDTOs;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace PaymentSvc.WebAPI.Controllers
{
    public class PayOSController(
        IPayOSService payOSService,
        PayOS payOS) : BaseController
    {
        private readonly IPayOSService _payOSService = payOSService;
        private readonly PayOS _payOS = payOS;

        [HttpPost("webhook")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PayOSWebhook([FromBody] WebhookType payload)
        {
            if (payload == null)
            {
                return Ok(new { code = -1, message = "empty payload" });
            }

            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(payload);
                await _payOSService.UpdatePaymentStatus(data.orderCode, PaymentStatusEnum.Success);

                return Ok(new { code = "00", message = "acknowledged" });
            }
            catch (Exception ex)
            {
                return Ok(new { code = -1, message = "handled with error: " + ex.Message });
            }
        }

        [HttpPut("cancel-payment/{transactionId}")]
        public async Task<IActionResult> CancelPayment(long transactionId)
        {
            await _payOSService.UpdatePaymentStatus(transactionId, PaymentStatusEnum.Cancelled);
            return Result.SuccessAction("Cancel payment successfully");
        }

        [HttpPut("complete-payment/{transactionId}")]
        public async Task<IActionResult> CompletePayment(long transactionId)
        {
            await _payOSService.UpdatePaymentStatus(transactionId, PaymentStatusEnum.Success);
            return Result.SuccessAction("Complete payment successfully");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePayOSDTO createPaymentDTO)
        {
            var result = await _payOSService.CreatePayOSPaymentAsync(createPaymentDTO);
            return Result.SuccessData(new GetPayOSDTO { TransactionId = result.TransactionId, PaymentUrl = result.PaymentUrl });
        }
    }
}