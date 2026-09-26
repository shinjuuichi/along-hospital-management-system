using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Services.Interfaces;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;

namespace SharedLibrary.Services.Implements
{
    public class TokenBlacklistService(ICacheService redis, IServiceProvider serviceProvider) : ITokenBlacklistService
    {
        private static readonly TimeSpan AuthAccountBlacklistTtl = TimeSpan.FromDays(1);
        private const string BlacklistKeyPrefix = "blacklist:";
        private const string AuthAccountBlacklistKeyPrefix = "blacklist:auth-account:";

        public async Task BlacklistAsync(string accessToken, DateTime? expiresAt = null)
        {
            var tokenData = this.ReadTokenData(accessToken);
            if (tokenData == null)
            {
                return;
            }

            var finalExpireTime = expiresAt ?? tokenData.ExpireAtUtc ?? DateTime.UtcNow.AddHours(1);

            var ttl = finalExpireTime - DateTime.UtcNow;
            if (ttl <= TimeSpan.Zero)
            {
                ttl = TimeSpan.FromMinutes(1);
            }

            await this.SetBlacklistEntryAsync(tokenData.Jti, ttl);
        }

        public async Task<bool> IsBlacklistedAsync(string accessToken)
        {
            var tokenData = this.ReadTokenData(accessToken);
            if (tokenData == null)
            {
                return false;
            }

            return await this.ExistsBlacklistEntryAsync(tokenData.Jti);
        }

        public async Task BlacklistAuthAccountAsync(int authAccountId)
        {
            await this.SetEntryAsync(this.BuildAuthAccountBlacklistKey(authAccountId), "1", AuthAccountBlacklistTtl);
        }

        public async Task<bool> IsAuthAccountBlacklistedAsync(int authAccountId)
        {
            return await this.ExistsEntryAsync(this.BuildAuthAccountBlacklistKey(authAccountId));
        }

        private async Task SetBlacklistEntryAsync(string jti, TimeSpan ttl)
        {
            await this.SetEntryAsync(this.BuildBlacklistKey(jti), "1", ttl);
        }

        private async Task<bool> ExistsBlacklistEntryAsync(string jti)
        {
            return await this.ExistsEntryAsync(this.BuildBlacklistKey(jti));
        }

        private async Task SetEntryAsync(string key, string value, TimeSpan? ttl = null)
        {
            var redisDatabase = this.GetRedisDatabase();

            if (redisDatabase != null)
            {
                await redisDatabase.StringSetAsync(key, value, ttl);
                return;
            }

            await redis.SetAsync(key, value, ttl);
        }

        private async Task<bool> ExistsEntryAsync(string key)
        {
            var redisDatabase = this.GetRedisDatabase();

            if (redisDatabase != null)
            {
                return await redisDatabase.KeyExistsAsync(key);
            }

            return await redis.ExistsAsync(key);
        }

        private TokenData? ReadTokenData(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();

            JwtSecurityToken? jwt;
            try
            {
                jwt = handler.ReadJwtToken(accessToken);
            }
            catch
            {
                return null;
            }

            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrWhiteSpace(jti))
            {
                return null;
            }

            return new TokenData(jti, this.TryGetTokenExpireTime(jwt));
        }

        private DateTime? TryGetTokenExpireTime(JwtSecurityToken jwt)
        {
            var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (long.TryParse(expClaim, out var expUnix))
            {
                return DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
            }

            return null;
        }

        private IDatabase? GetRedisDatabase()
        {
            return serviceProvider.GetService<IConnectionMultiplexer>()?.GetDatabase();
        }

        private string BuildBlacklistKey(string jti) => $"{BlacklistKeyPrefix}{jti}";
        private string BuildAuthAccountBlacklistKey(int authAccountId) => $"{AuthAccountBlacklistKeyPrefix}{authAccountId}";

        private sealed record TokenData(string Jti, DateTime? ExpireAtUtc);
    }

}