using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Contratcs
{
   public interface IRegexBuilder
    {
        string Build(string pattern);
    }
}
