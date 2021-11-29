using ApiRateLimiter.Common.Contratcs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Implementations
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Set<T>(string key, T data, DateTimeOffset expireTime)
        {
            _memoryCache.Set(key, data, expireTime);
        }
    }
}
