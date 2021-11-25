using ApiRateLimiter.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Options
{
    public class ApiRateLimiterOption
    {
        public Dictionary<string, RouteLimitationOption> ApiUrls { get; set; }
        public HashSet<string> WhiteList { get; set; }
    }
    public class RouteLimitationOption
    {
        public int Limit { get; set; }
        public int Duration { get; set; }
    }
}
