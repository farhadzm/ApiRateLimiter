using System;
using System.Collections.Generic;
using System.Text;

namespace ApiRateLimiter.Common.Helpers
{
   public static class DictionaryHelper
    {
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                         TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
    }
}
