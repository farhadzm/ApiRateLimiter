using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Contratcs
{
   public interface ICache
    {
        void Set<T>(string key, T data, DateTimeOffset expireTime);
        T Get<T>(string key);
    }
}
