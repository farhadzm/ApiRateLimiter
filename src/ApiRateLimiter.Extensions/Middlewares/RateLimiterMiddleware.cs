using ApiRateLimiter.Common.Contratcs;
using ApiRateLimiter.Common.Helpers;
using ApiRateLimiter.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiRateLimiter.Extensions.Middlewares
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICache _cache;
        private readonly ILogger<RateLimiterMiddleware> _logger;
        private readonly ILimitationChecker _limitationChecker;
        private readonly IApiRateLimiter _apiRateLimiter;

        public RateLimiterMiddleware(RequestDelegate next, ICache cache, ILogger<RateLimiterMiddleware> logger, ILimitationChecker limitationChecker, IApiRateLimiter apiRateLimiter)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _limitationChecker = limitationChecker;
            _apiRateLimiter = apiRateLimiter;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string route = httpContext.Request.Path.ToString();
            string ip = httpContext.Connection.RemoteIpAddress.ToString();

            var hasLimitationOnRoute = HasLimitationOnRoute(route, ip, out string limitationRoute);
            if (hasLimitationOnRoute)
            {
                var reachedToLimitation = await _apiRateLimiter.IsReachedToLimitation(route, ip, limitationRoute);
                if (reachedToLimitation)
                {
                    httpContext.Response.StatusCode = 429;
                    await httpContext.Response.WriteAsync("Too many requests");
                    _logger.LogWarning($"Request blocked. IP:\"{ip}\", Route:\"{route}\"");
                    return;
                }
            }
            await _next(httpContext);
        }
        private bool HasLimitationOnRoute(string route, string ip, out string routeRegEx)
        {
            var limitationRoute = new LimitationRoute();
            var key = KeyHelper.GenerateLimitationRouteCacheKey(route, ip);
            var result = _cache.Get<LimitationRoute>(key);
            if (result != null)
            {
                routeRegEx = result.RouteRegEx;
                return result.HasLimitation;
            }

            var hasLimitation = _limitationChecker.HasLimitationOnRoute(route, ip, out routeRegEx);
            limitationRoute.RouteRegEx = routeRegEx;
            limitationRoute.HasLimitation = hasLimitation;

            SetResultInMemoryCache(key, limitationRoute);
            return hasLimitation;

            void SetResultInMemoryCache(string key, LimitationRoute result)
            {
                _cache.Set(key, result, DateTimeOffset.UtcNow.AddDays(1));
            }
        }
    }
}
