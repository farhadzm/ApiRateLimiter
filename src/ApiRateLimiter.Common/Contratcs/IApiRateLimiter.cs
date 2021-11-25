using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiRateLimiter.Common.Contratcs
{
    public interface IApiRateLimiter
    {
        ValueTask<bool> IsReachedToLimitation(string route, string ip,string routeRegEx);
    }
}
