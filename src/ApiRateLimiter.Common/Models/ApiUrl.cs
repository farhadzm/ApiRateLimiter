using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Models
{
    public class ApiUrl
    {
        public ApiUrl(string route)
        {
            Route = route;
        }

        public string Route { get; set; }
        public int Limit { get; set; }
        public override bool Equals(object obj)
        {
            if(obj is ApiUrl apiUrlObj)
            {
                if(Route.Equals(apiUrlObj.Route, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Route);
        }

        public static implicit operator ApiUrl(string route)
        {
            return new ApiUrl(route);
        }
    }
}
