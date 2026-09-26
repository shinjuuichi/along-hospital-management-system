using Microsoft.AspNetCore.Http;

namespace AuthSvc.BLL.Interfaces
{
    public interface IRefreshTokenCookieService
    {
        void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expiresAt);
        string? GetRefreshTokenFromCookie(HttpRequest request);
        void ClearRefreshTokenCookie(HttpResponse response);
    }
}
