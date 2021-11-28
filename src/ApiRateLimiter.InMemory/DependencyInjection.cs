using ApiRateLimiter.Common.Contratcs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.InMemory
{
    public static class DependencyInjection
    {
        public static void AddInMemoryRateLimiter(this IServiceCollection services)
        {
            services.AddSingleton<IApiRateLimiter, InMemoryRateLimiter>();
        }
    }
}
