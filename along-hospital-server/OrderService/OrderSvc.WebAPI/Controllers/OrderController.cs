using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSvc.BLL.DTOs;
using OrderSvc.BLL.Interfaces;
using OrderSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace OrderSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class OrderController(IOrderService orderService) : BaseController
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders(OrderFilterDTO orderFilterDTO)
        {
            var result = await _orderService.GetAllPaginatedByUserIdAsync(orderFilterDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var result = await _orderService.GetOrderByCurrentUserAsync(orderId);
            return Result.SuccessData(result);
        }

        [HttpPut("cancel/{orderId}")]
        public async Task<IActionResult> CancelledOrder(int orderId)
        {
            await _orderService.CancelOrderAsync(orderId);
            return Result.SuccessAction("Order cancelled successfully.");
        }

        [HttpPut("repay/{orderId}")]
        public async Task<IActionResult> RepayOrder(int orderId, string paymentType)
        {
            var paymentUrl = await _orderService.RepayOrderAsync(orderId, paymentType);
            return Result.SuccessData(paymentUrl);
        }
    }
}