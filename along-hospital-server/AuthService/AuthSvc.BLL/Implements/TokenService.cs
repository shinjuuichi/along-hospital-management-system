using AuthSvc.BLL.DTOs.Response;
using AuthSvc.BLL.Interfaces;
using AuthSvc.DAL.Enums;
using AuthSvc.BLL.Utils;
using AuthSvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using System.Security.Claims;

namespace AuthSvc.BLL.Implements
{
    public class TokenService(
        IUnitOfWork _unitOfWork,
        IMessageBus _bus,
        AppConfiguration _configuration,
        ITokenBlacklistService _tokenBlacklistService) : ITokenService
    {
        private IGenericRepository<RefreshToken> _refreshTokenRepository => _unitOfWork.Repository<RefreshToken>();

        public async Task<GetAuthResponseDTO> GenerateTokensAsync(AuthAccount auth)
        {
            this.EnsureAuthAccountCanIssueTokens(auth);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, (auth.UserId ?? 0).ToString()),
                new(ClaimTypes.Email, auth.Email ?? string.Empty),
                new("AuthId", auth.Id.ToString()),
                new("Stage", auth.Stage.ToString())
            };

            string? role = null;

            var isPendingStage = auth.Stage == AuthStageEnum.PatientProfilePendingWithPhone
                                  || auth.Stage == AuthStageEnum.PatientProfilePendingWithoutPhone;
            if (!isPendingStage)
            {
                var response = await _bus.RequestAsync<GetUserRoleByUserIdEvent, GetUserRoleByUserIdContract>(
                    new GetUserRoleByUserIdEvent { Id = auth.UserId ?? 0 });
                role = response.Role;
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var (accessToken, accessTokenExpires) = JwtUtil.GenerateAccessToken(claims, _configuration.JwtConfig);
            var refreshTokenExpirationDays = _configuration.RefreshTokenConfig?.ExpirationDays ?? 7;
            var (refreshToken, refreshTokenExpires) = JwtUtil.GenerateRefreshToken(refreshTokenExpirationDays);

            var refreshTokenHash = CryptoUtil.GetSha256Hash(refreshToken);
            var newRefreshToken = new RefreshToken
            {
                AuthAccountId = auth.Id,
                RefreshTokenHash = refreshTokenHash,
                ExpiresAt = refreshTokenExpires
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangeAsync();

            return new GetAuthResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpires = accessTokenExpires,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpires,
                Role = role,
                AuthId = auth.Id,
                UserId = auth.UserId,
                Stage = auth.Stage.ToString()
            };
        }

        public async Task<GetAuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenHash = CryptoUtil.GetSha256Hash(refreshToken);

            var existingRefreshToken = await _refreshTokenRepository
                .GetByConditionAsync(rt => rt.RefreshTokenHash == refreshTokenHash, includes: ["AuthAccount"]);

            if (existingRefreshToken == null || existingRefreshToken.ExpiresAt <= DateTime.UtcNow || existingRefreshToken.IsDeleted)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = existingRefreshToken.AuthAccount;
            await this.EnsureRefreshTokenCanBeUsedAsync(user);

            await this.RevokeTokenAsync(refreshToken);

            return await this.GenerateTokensAsync(user);
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var refreshTokenHash = CryptoUtil.GetSha256Hash(refreshToken);
            var existingRefreshToken = await _refreshTokenRepository
                .GetByConditionAsync(rt => rt.RefreshTokenHash == refreshTokenHash);

            if (existingRefreshToken != null)
            {
                _refreshTokenRepository.Remove(existingRefreshToken);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task RevokeAllTokensAsync(int authAccountId)
        {
            var existingRefreshTokens = await _refreshTokenRepository.GetAllAsync(rt => rt.AuthAccountId == authAccountId);
            if (existingRefreshTokens.Count == 0)
            {
                return;
            }

            foreach (var token in existingRefreshTokens)
            {
                _refreshTokenRepository.Remove(token);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task BlacklistAccessTokenAsync(string accessToken)
        {
            await _tokenBlacklistService.BlacklistAsync(accessToken);
        }

        private async Task EnsureRefreshTokenCanBeUsedAsync(AuthAccount auth)
        {
            if (auth.Status == AuthStatusEnum.Verified)
            {
                return;
            }

            await this.RevokeAllTokensAsync(auth.Id);

            if (auth.Status == AuthStatusEnum.Terminated
                || auth.Status == AuthStatusEnum.Suspended)
            {
                await _tokenBlacklistService.BlacklistAuthAccountAsync(auth.Id);
            }

            this.EnsureAuthAccountCanIssueTokens(auth);
        }

        private void EnsureAuthAccountCanIssueTokens(AuthAccount auth)
        {
            if (auth.Status == AuthStatusEnum.Terminated
                || auth.Status == AuthStatusEnum.Suspended)
            {
                throw new UnauthorizedAccessException("Your account is terminated or suspended");
            }

            if (auth.Status != AuthStatusEnum.Verified)
            {
                throw new UnauthorizedAccessException("Your account is not verified");
            }
        }
    }
}
