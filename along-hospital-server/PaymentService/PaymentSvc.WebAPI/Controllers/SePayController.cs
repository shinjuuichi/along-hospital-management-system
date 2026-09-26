using Microsoft.AspNetCore.Mvc;
using PaymentSvc.BLL.DTOs.SePayDTOs;
using PaymentSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace PaymentSvc.WebAPI.Controllers
{
    public class SePayController(ISePayService sePayService) : BaseController
    {
        private readonly ISePayService _sePayService = sePayService;

        [HttpPost("webhook")]
        public async Task<IActionResult> VerifyIpn([FromBody] SePayIPNRequestDTO sePayIPNRequestDTO)
        {
            await _sePayService.ProcessIPNAsync(sePayIPNRequestDTO);
            return Result.SuccessAction("Ipn processed successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSePayPayment([FromBody] CreateSePayDTO createSePayDTO)
        {
            var result = await _sePayService.CreateSePayPaymentAsync(createSePayDTO);
            return Result.SuccessData(result);
        }
    }
}