using AuthSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons;

namespace AuthSvc.BLL.Implements
{
    public class RefreshTokenCookieService(AppConfiguration _configuration) : IRefreshTokenCookieService
    {
        private const string RefreshTokenCookieName = "refresh_token";

        public void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expiresAt)
        {
            var isSecure = _configuration.RefreshTokenConfig?.Secure ?? true;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure,
                SameSite = isSecure ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = expiresAt,
                Path = "/",
                Domain = string.IsNullOrEmpty(_configuration.RefreshTokenConfig?.Domain)
                    ? null
                    : _configuration.RefreshTokenConfig.Domain
            };

            response.Cookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
        }

        public string? GetRefreshTokenFromCookie(HttpRequest request)
        {
            return request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken)
                ? refreshToken
                : null;
        }

        public void ClearRefreshTokenCookie(HttpResponse response)
        {
            var isSecure = _configuration.RefreshTokenConfig?.Secure ?? true;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isSecure,
                SameSite = isSecure ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/",
                Domain = string.IsNullOrEmpty(_configuration.RefreshTokenConfig?.Domain)
                    ? null
                    : _configuration.RefreshTokenConfig.Domain
            };

            response.Cookies.Append(RefreshTokenCookieName, string.Empty, cookieOptions);
        }
    }
}
