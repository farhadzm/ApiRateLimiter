using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Extensions.Middlewares
{
    public static class RateLimiterMiddlewareExtension
    {
        public static void UseRateLimiter(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
