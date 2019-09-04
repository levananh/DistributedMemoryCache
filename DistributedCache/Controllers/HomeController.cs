using CacheService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DistributedCache.Models;

namespace DistributedCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataCache cache;

        public HomeController(IDataCache dataCache)
        {
            this.cache = dataCache;
        }

        public async Task<IActionResult> Index()
        {
            await TestCacheDate();
            await TestCacheNumber();
            await CacheObject();

            return View();
        }

        private async Task CacheObject()
        {
            var x = new Random().Next(); // chỉ để test
            ViewBag.Doctor = await cache.GetAsync<Doctor>("doctor", () => new Doctor()
            {
                Id = x,
                Email = "anh.levan108@gmail.com",
                Name = $"Le Anh {DateTime.Now:F}"
            });
        }

        private async Task TestCacheNumber()
        {
            const string key = "number_cache";
            await cache.SetAsync(key, DateTime.Now.Second);
            ViewBag.Number = await cache.GetAsync<int?>(key);
        }

        private async Task TestCacheDate()
        {
            const string key = "cache_date";
            await cache.SetAsync(key, DateTime.Now);
            ViewBag.CacheData = await cache.GetAsync<DateTime>(key);
        }
    }
}
