using CartSvc.BLL.DTOs;
using CartSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace CartSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class CartController(ICartService cartservice) : BaseController
    {
        private readonly ICartService _cartService = cartservice;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartByCurrentUser()
        {
            var result = await _cartService.GetCurrentUserCartAsync();
            return Result.SuccessData(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateDetail(UpsertCartDetailDTO dto)
        {
            await _cartService.UpdateDetailAsync(dto);
            return Result.SuccessAction("The item has been updated in your cart.");
        }

        [HttpDelete("delete/{skuCode}")]
        public async Task<IActionResult> DeleteDetail(string skuCode)
        {
            await _cartService.DeleteDetailAsync(skuCode);
            return Result.SuccessAction("The item has been removed from your cart.");
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(UpsertCartDetailDTO dto)
        {
            await _cartService.AddToCartAsync(dto);
            return Result.SuccessAction("The item added to cart successfully");
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutDTO checkoutDTO)
        {
            var paymentUrl = await _cartService.CheckoutAsync(checkoutDTO);
            return Result.SuccessData(paymentUrl, "Your order has been processed successfully.");
        }
    }
}
