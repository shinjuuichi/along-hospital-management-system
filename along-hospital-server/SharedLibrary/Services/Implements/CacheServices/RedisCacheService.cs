using Microsoft.Extensions.Caching.Distributed;
using SharedLibrary.Services.Interfaces;
using StackExchange.Redis;
using System.Text;

namespace SharedLibrary.Services.Implements.CacheServices
{
    public class RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : ICacheService
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {

            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task<string?> GetAsync(string key)
        {
            var bytes = await distributedCache.GetAsync(key);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        public async Task DeleteAsync(string key)
        {
            await distributedCache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var value = await GetAsync(key);
            return value != null;
        }

        public Task<long> IncrementAsync(string key) => _database.StringIncrementAsync(key);

        public Task<bool> ExpireAsync(string key, TimeSpan expiry) => _database.KeyExpireAsync(key, expiry);
    }
}
