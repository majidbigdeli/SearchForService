using System;
using System.Threading.Tasks;
using MethodTimer;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SearchForApi.Utilities.LockManager;

namespace SearchForApi.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [Time]
        public async Task SetItem<T>(string key, T value, DateTime expireDate)
        {
            using (var lockManager = new CacheLockManager())
            {
                await lockManager.AcquireLock(key);

                var normalizedValue = JsonConvert.SerializeObject(value);
                await _distributedCache.SetStringAsync(key, normalizedValue, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = expireDate
                });
            }
        }

        [Time]
        public async Task SetItem<T>(string key, T value, TimeSpan expireDate)
        {
            using (var lockManager = new CacheLockManager())
            {
                await lockManager.AcquireLock(key);

                var normalizedValue = JsonConvert.SerializeObject(value);
                await _distributedCache.SetStringAsync(key, normalizedValue, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expireDate
                });
            }
        }

        [Time]
        public async Task<T> GetItem<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (value == null) return default(T);

            var result = JsonConvert.DeserializeObject<T>(value);
            return result;
        }

        [Time]
        public async Task RemoveItem(string key)
        {
            using (var lockManager = new CacheLockManager())
            {
                await lockManager.AcquireLock(key);

                try
                {
                    await _distributedCache.RemoveAsync(key);
                }
                catch { }
            }
        }
    }

    public class CacheLockManager : LockManager<string>
    {
        public CacheLockManager() : base("Cache") { }
    }
}