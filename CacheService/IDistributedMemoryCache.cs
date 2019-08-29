using System;
using System.Threading.Tasks;

namespace CacheService
{
    public interface IDistributedMemoryCache
    {
        Task SetAsync(string key, string value);

        Task SetAsync<T>(string key, T value);

        Task<string> GetAsync(string key);
    }
}
