using ApiRateLimiter.Common.Contratcs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiRateLimiter.Extensions.Middlewares
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimiterMiddleware> _logger;
        private readonly ILimitationChecker _limitationChecker;
        private readonly IApiRateLimiter _apiRateLimiter;

        public RateLimiterMiddleware(RequestDelegate next, ILogger<RateLimiterMiddleware> logger, ILimitationChecker limitationChecker, IApiRateLimiter apiRateLimiter)
        {
            _next = next;
            _logger = logger;
            _limitationChecker = limitationChecker;
            _apiRateLimiter = apiRateLimiter;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string route = httpContext.Request.Path.ToString();
            string ip = httpContext.Connection.RemoteIpAddress.ToString();

            var hasLimitationOnRoute = _limitationChecker.HasLimitationOnRoute(route, ip, out string routeRegEx);
            if (hasLimitationOnRoute)
            {
                var reachedToLimitation = await _apiRateLimiter.IsReachedToLimitation(route, ip, routeRegEx);
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
    }
}
