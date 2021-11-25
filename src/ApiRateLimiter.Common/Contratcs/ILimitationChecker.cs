using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Contratcs
{
    public interface ILimitationChecker
    {
        bool HasLimitationOnRoute(string route, string ip,out string routeRegEx);
    }
}
