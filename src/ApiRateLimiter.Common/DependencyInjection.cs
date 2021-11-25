
using ApiRateLimiter.Common.Contratcs;
using ApiRateLimiter.Common.Implementations;
using ApiRateLimiter.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace ApiRateLimiter.Common
{
    public static class DependencyInjection
    {
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                         TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
        public static void AddApiRateLimiter(this IServiceCollection services, IConfiguration configuration)
        {
            var limitOption = configuration.GetSection(nameof(ApiRateLimiterOption)).Get<ApiRateLimiterOption>();
            if (limitOption.ApiUrls != null && limitOption.ApiUrls.Keys.Any())
            {
                var keys = limitOption.ApiUrls.Keys.ToList();
                foreach (var item in keys)
                {
                    limitOption.ApiUrls.RenameKey(item, RegexBuilder.Create(item));
                }
            }
            services.Configure<ApiRateLimiterOption>(a =>
            {
                a.ApiUrls = limitOption.ApiUrls;
                a.WhiteList = limitOption.WhiteList;
            });
            services.AddSingleton<ILimitationChecker, LimitationChecker>();
            services.AddSingleton<IRegexBuilder, RegexBuilder>();
        }
    }
}
