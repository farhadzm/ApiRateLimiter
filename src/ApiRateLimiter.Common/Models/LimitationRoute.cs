using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Models
{
   public class LimitationRoute
    {
        public LimitationRoute()
        {

        }
        public LimitationRoute(string route, bool hasLimitation)
        {
            RouteRegEx = route;
            HasLimitation = hasLimitation;
        }

        public string RouteRegEx { get; set; }
        public bool HasLimitation { get; set; }
    }
}
