using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;

namespace AuthSvc.WebAPI.Controllers
{
    public class AuthController(
        IAuthService _authService,
        ITokenService _tokenService,
        IPasswordService _passwordService,
        IRefreshTokenCookieService _cookieService) : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            await _authService.RegisterAsync(request);
            return Result.SuccessAction("Registration initiated. We sent the first verification to your email");
        }

        [HttpPost("register/resend/options")]
        public async Task<IActionResult> GetRegisterResendOptions(LookupVerificationMethodsRequestDTO request)
        {
            var result = await _authService.GetRegisterResendOptionsAsync(request);
            return Result.SuccessData(result);
        }

        [HttpPost("register/resend")]
        public async Task<IActionResult> ResendRegister(ResendRegisterRequestDTO request)
        {
            await _authService.ResendRegisterAsync(request);
            return Result.SuccessAction("A new verification message has been sent");
        }

        [HttpPost("register/verify")]
        public async Task<IActionResult> VerifyRegister(VerifyRegisterRequestDTO request)
        {
            var result = await _authService.VerifyRegisterAsync(request);
            _cookieService.SetRefreshTokenCookie(Response, result.RefreshToken!, result.RefreshTokenExpires!.Value);

            return Result.SuccessData(result, "Verify account successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            _cookieService.SetRefreshTokenCookie(Response, result.RefreshToken!, result.RefreshTokenExpires!.Value);

            return Result.SuccessData(result, "Login successfully");
        }

        [HttpPost("login/google")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginRequestDTO request)
        {
            var result = await _authService.GoogleLoginAsync(request);
            _cookieService.SetRefreshTokenCookie(Response, result.RefreshToken!, result.RefreshTokenExpires!.Value);

            return Result.SuccessData(result, "Login successfully");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = _cookieService.GetRefreshTokenFromCookie(Request);
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _cookieService.ClearRefreshTokenCookie(Response);
                throw new ForbiddenException("Refresh token is missing");
            }

            var result = await _tokenService.RefreshTokenAsync(refreshToken);
            _cookieService.SetRefreshTokenCookie(Response, result.RefreshToken!, result.RefreshTokenExpires!.Value);

            return Result.SuccessData(result);
        }

        [HttpPost("forgot-password/options")]
        public async Task<IActionResult> GetForgotPasswordOptions(LookupVerificationMethodsRequestDTO request)
        {
            var result = await _passwordService.GetForgotPasswordOptionsAsync(request);
            return Result.SuccessData(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordRequest(ForgotPasswordRequestDTO request)
        {
            await _passwordService.ForgotPasswordRequestAsync(request);
            return Result.SuccessAction("Reset instructions have been sent to your selected method");
        }

        [HttpPost("forgot-password/verify")]
        public async Task<IActionResult> VerifyForgotPassword(VerifyForgotPasswordRequestDTO request)
        {
            var result = await _passwordService.VerifyForgotPasswordAsync(request);
            return Result.SuccessData(result, "Verification successful");
        }

        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO request)
        {
            await _passwordService.ResetPasswordAsync(request);
            return Result.SuccessAction("Password has been reset successfully");
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _authService.GetMeAsync();
            return Result.SuccessData(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();
            var accessToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : string.Empty;
            var refreshToken = _cookieService.GetRefreshTokenFromCookie(Request);

            await _authService.LogoutAsync(accessToken, refreshToken);
            _cookieService.ClearRefreshTokenCookie(Response);

            return Result.SuccessAction("Logged out successfully");
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDTO request)
        {
            var authHeader = HttpContext.Request.Headers.Authorization.ToString();
            var accessToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : string.Empty;
            await _passwordService.ChangePasswordAsync(request, accessToken);
            _cookieService.ClearRefreshTokenCookie(Response);
            return Result.SuccessAction("Password changed successfully. Please log in again");
        }
    }
}
