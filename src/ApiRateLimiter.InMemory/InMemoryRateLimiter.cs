using ApiRateLimiter.Common.Contratcs;
using ApiRateLimiter.Common.Helpers;
using ApiRateLimiter.Common.Models;
using ApiRateLimiter.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiRateLimiter.InMemory
{
    public class InMemoryRateLimiter : IApiRateLimiter
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ApiRateLimiterOption _option;
        public InMemoryRateLimiter(IOptions<ApiRateLimiterOption> options, IMemoryCache memoryCache)
        {
            _option = options.Value;
            _memoryCache = memoryCache;
        }
        public ValueTask<bool> IsReachedToLimitation(string route, string ip, string routeRegEx)
        {
            var routeLimitation = _option.ApiUrls[routeRegEx];
            var key = KeyHelper.GenerateCacheKey(route, ip);

            if (_memoryCache.TryGetValue(key, out int requestCount))
            {
                if (routeLimitation.Limit < requestCount)
                {
                    return new ValueTask<bool>(true);
                }
                requestCount++;
                SetRequestCount(key, requestCount, routeLimitation.Duration);
                return new ValueTask<bool>(false);
            }
            else
            {
                SetRequestCount(key, 1, routeLimitation.Duration);
                return new ValueTask<bool>(false);
            }
        }

        private void SetRequestCount(string key, int requestCount, int seconds)
        {
            var expireDate = DateTimeOffset.UtcNow.AddSeconds(seconds);
            _memoryCache.Set(key, requestCount, expireDate);
        }
    }
}
