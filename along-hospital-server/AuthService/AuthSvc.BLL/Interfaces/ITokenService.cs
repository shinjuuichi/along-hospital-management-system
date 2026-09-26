using AuthSvc.BLL.DTOs.Response;
using AuthSvc.DAL.Models;

namespace AuthSvc.BLL.Interfaces
{
    public interface ITokenService
    {
        Task<GetAuthResponseDTO> GenerateTokensAsync(AuthAccount auth);
        Task<GetAuthResponseDTO> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);
        Task RevokeAllTokensAsync(int authAccountId);
        Task BlacklistAccessTokenAsync(string accessToken);
    }
}
