using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSvc.BLL.DTOs;
using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace OrderSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Pharmacist))]
    public class OrderManagementController(IOrderService orderService) : GetController<GetOrderDTO, OrderFilterDTO>(orderService)
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDTO orderCreateDTO)
        {
            var createdOrder = await _orderService.CreateAsync(orderCreateDTO);
            return Result.SuccessData(createdOrder, "Order created successfully.");
        }

        [HttpPut("shipping/{orderId}")]
        public async Task<IActionResult> Shipping(int orderId)
        {
            await _orderService.ShippingOrderAsync(orderId);
            return Result.SuccessAction("Order marked as shipping successfully.");
        }

        [HttpPut("paid/{orderId}")]
        public async Task<IActionResult> PaidOrder(int orderId)
        {
            await _orderService.PaidOrderAsync(orderId);
            return Result.SuccessAction("Order marked as paid successfully.");
        }

        [HttpPut("complete/{orderId}")]
        public async Task<IActionResult> Complete(int orderId)
        {
            await _orderService.CompleteOrderAsync(orderId);
            return Result.SuccessAction("Order marked as completed successfully.");
        }
    }
}
