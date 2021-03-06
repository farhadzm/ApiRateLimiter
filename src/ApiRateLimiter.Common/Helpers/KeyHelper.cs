using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Helpers
{
   public static class KeyHelper
    {
        public static string GenerateCacheKey(string route,string ip)
        {
            return $"{route.ToLowerInvariant()}_{ip.ToUpperInvariant()}";
        }
        public static string GenerateLimitationRouteCacheKey(string route, string ip)
        {
            return $"LIMITATION_{route.ToLowerInvariant()}_{ip.ToUpperInvariant()}";
        }
    }
}
