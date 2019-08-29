using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CacheService;
using Microsoft.AspNetCore.Mvc;
using DistributedCache.Models;

namespace DistributedCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistributedMemoryCache cache;

        public HomeController(IDistributedMemoryCache distributedMemoryCache)
        {
            this.cache = distributedMemoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var cachedData = await cache.GetAsync("test");
            if (string.IsNullOrWhiteSpace(cachedData))
            {
                await cache.SetAsync("test", DateTime.Now.ToString("F"));
                cachedData = await cache.GetAsync("test");
            }

            ViewBag.CacheData = cachedData;

            return View();
        }
    }
}
