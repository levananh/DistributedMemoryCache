using System;
using System.Threading.Tasks;

namespace CacheService
{
    public interface IDistributedMemoryCache
    {
        Task SetAsync(string key, string value);

        Task<string> GetAsync(string key);
    }
}
