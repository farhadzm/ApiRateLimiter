using ApiRateLimiter.Common.Contratcs;
using ApiRateLimiter.Common.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ApiRateLimiter.Common.Implementations
{
    public class LimitationChecker : ILimitationChecker
    {
        private readonly ApiRateLimiterOption _option;
        public LimitationChecker(IOptions<ApiRateLimiterOption> options)
        {
            _option = options.Value;
        }
        public bool HasLimitationOnRoute(string route, string ip, out string routeRegEx)
        {
            routeRegEx = string.Empty;
            if (IpIsInWhiteList(ip))
            {
                return false;
            }
            if (_option.ApiUrls != null)
            {
                foreach (var item in _option.ApiUrls.Keys)
                {
                    if (Regex.IsMatch(route, item))
                    {
                        routeRegEx = item;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool IpIsInWhiteList(string ip)
        {
            var isInWhiteList = _option.WhiteList?.Contains(ip) ?? false;

            return isInWhiteList;
        }
    }
}
