using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CacheService
{
    public class DataCache : IDataCache
    {
        private readonly IDistributedCache distributedCache;

        public DataCache(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        /// <inheritdoc />
        public async Task SetAsync<T>(string key, T value, bool? overrideExistedValue = null)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
            {
                throw new ArgumentException("Invalid key or value");
            }

            if (overrideExistedValue != true && await distributedCache.GetAsync(key) != null)
            {
                return;
            }

            var jsonValue = JsonConvert.SerializeObject(value);
            await CacheStringAsync(key, jsonValue);
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key must not be null");
            }

            var cachedValue = await distributedCache.GetAsync(key);
            return cachedValue == null ? default(T) : JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(cachedValue));
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                await distributedCache.RemoveAsync(key);
            }
        }

        /// <inheritdoc />
        public async Task RefreshAsync(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                await distributedCache.RefreshAsync(key);
            }
        }

        private async Task CacheStringAsync(string key, string value)
        {
            var cacheOption = new DistributedCacheEntryOptions
            {
                // Thời gian cache, để tạm 10 ngay. Tùy từng bài toán cụ thể sẽ có thời gian cache khác nhau
                AbsoluteExpiration = DateTime.Now.AddDays(10)
            };

            await distributedCache.SetAsync(key, Encoding.ASCII.GetBytes(value), cacheOption);
        }
    }
}
