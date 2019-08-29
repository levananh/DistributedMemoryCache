using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CacheService
{
    public class DistributedMemoryCache : IDistributedMemoryCache
    {
        private readonly IDistributedCache distributedCache;

        public DistributedMemoryCache(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task SetAsync(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var cacheOption = new DistributedCacheEntryOptions
            {
                // Thời gian cache, để tạm 1 năm. Tùy từng bài toán cụ thể sẽ có thời gian cache khác nhau
                AbsoluteExpiration = DateTime.Now.AddYears(1) 
            };
            
            await distributedCache.SetAsync(key, Encoding.ASCII.GetBytes(value), cacheOption);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            await SetAsync(key, jsonValue);
        }

        public async Task<string> GetAsync(string key)
        {
            var cachedValue = await distributedCache.GetAsync(key);
            return cachedValue != null ? Encoding.ASCII.GetString(cachedValue) : null;
        }
    }
}
