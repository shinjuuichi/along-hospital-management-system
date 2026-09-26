using Microsoft.Extensions.Caching.Distributed;
using SharedLibrary.Services.Interfaces;
using System.Collections.Concurrent;
using System.Text;

namespace SharedLibrary.Services.Implements.CacheServices
{
    public class MemoryCacheService(IDistributedCache cache) : ICacheService
    {
        private readonly ConcurrentDictionary<string, long> _counters = new();

        public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            await cache.SetAsync(key, Encoding.UTF8.GetBytes(value), options);
        }

        public async Task<string?> GetAsync(string key)
        {
            var bytes = await cache.GetAsync(key);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        public Task DeleteAsync(string key)
        {
            _counters.TryRemove(key, out _);
            return cache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key) =>
            await GetAsync(key) is not null;

        public Task<long> IncrementAsync(string key)
        {
            var newValue = _counters.AddOrUpdate(key, 1, (_, oldValue) => oldValue + 1);
            return Task.FromResult(newValue);
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan expiry)
        {
            await Task.Delay(expiry);
            _counters.TryRemove(key, out _);
            await cache.RemoveAsync(key);
            return true;
        }
    }
}