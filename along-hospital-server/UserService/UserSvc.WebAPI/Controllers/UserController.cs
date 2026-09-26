using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Controllers
{
    [Authorize]
    public class UserController(
    IUserService _userService, ITokenBlacklistService _tokenBlacklistService)
        : BaseController
    {
        [HttpPost("complete-profile")]
        [Authorize(Policy = "ProfilePending")]
        public async Task<IActionResult> CompleteProfile(CreateUserWithRolePatientProfileDTO dto)
        {
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();
            var accessToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : string.Empty;
            await _tokenBlacklistService.BlacklistAsync(accessToken);

            await _userService.CompleteProfileAsync(dto);
            return Result.SuccessAction("Profile completed successfully");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _userService.GetUserProfileAsync();
            return Result.SuccessData(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileUserDTO dto)
        {
            await _userService.UpdateProfileAsync(dto);
            return Result.SuccessAction("Profile updated successfully");
        }
    }
}
