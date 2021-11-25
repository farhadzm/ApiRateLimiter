using ApiRateLimiter.Common.Contratcs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiRateLimiter.Common.Implementations
{
    public class RegexBuilder : IRegexBuilder
    {
        private readonly IMemoryCache _memoryCache;
        private const string RegExMatchOneOrMoreOfEverything = ".+";
        private const string RegExMatchOneOrMoreOfEverythingUntilNextForwardSlash = "[^/]+";
        private const string RegExMatchEndString = "$";
        private const string RegExIgnoreCase = "(?i)";
        private const string RegExForwardSlashOnly = "^/$";
        private const string RegExForwardSlashAndOnePlaceHolder = "^/.*";

        public RegexBuilder(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public string Build(string pattern)
        {
            if (_memoryCache.TryGetValue(pattern, out string result))
            {
                return result;
            }
            result = Create(pattern);
            _memoryCache.Set(pattern, result, DateTimeOffset.UtcNow.AddHours(1));
            return result;
        }
        public static string Create(string route)
        {
            var upstreamTemplate = route;

            var placeholders = new List<string>();

            for (var i = 0; i < upstreamTemplate.Length; i++)
            {
                if (IsPlaceHolder(upstreamTemplate, i))
                {
                    var postitionOfPlaceHolderClosingBracket = upstreamTemplate.IndexOf('}', i);
                    var difference = postitionOfPlaceHolderClosingBracket - i + 1;
                    var placeHolderName = upstreamTemplate.Substring(i, difference);
                    placeholders.Add(placeHolderName);

                    if (ForwardSlashAndOnePlaceHolder(upstreamTemplate, placeholders, postitionOfPlaceHolderClosingBracket))
                    {
                        return RegExForwardSlashAndOnePlaceHolder;
                    }
                }
            }

            var containsQueryString = false;

            if (upstreamTemplate.Contains("?"))
            {
                containsQueryString = true;
                upstreamTemplate = upstreamTemplate.Replace("?", "\\?");
            }

            for (int i = 0; i < placeholders.Count; i++)
            {
                var indexOfPlaceholder = upstreamTemplate.IndexOf(placeholders[i]);
                var indexOfNextForwardSlash = upstreamTemplate.IndexOf("/", indexOfPlaceholder);
                if (indexOfNextForwardSlash < indexOfPlaceholder || (containsQueryString && upstreamTemplate.IndexOf("?") < upstreamTemplate.IndexOf(placeholders[i])))
                {
                    upstreamTemplate = upstreamTemplate.Replace(placeholders[i], RegExMatchOneOrMoreOfEverything);
                }
                else
                {
                    upstreamTemplate = upstreamTemplate.Replace(placeholders[i], RegExMatchOneOrMoreOfEverythingUntilNextForwardSlash);
                }
            }

            if (upstreamTemplate == "/")
            {
                return RegExForwardSlashOnly;
            }

            if (upstreamTemplate.EndsWith("/"))
            {
                upstreamTemplate = upstreamTemplate.Remove(upstreamTemplate.Length - 1, 1) + "(/|)";
            }

            var template = $"^{RegExIgnoreCase}{upstreamTemplate}{RegExMatchEndString}";

            return template;
        }

        private static bool ForwardSlashAndOnePlaceHolder(string upstreamTemplate, List<string> placeholders, int postitionOfPlaceHolderClosingBracket)
        {
            if (upstreamTemplate.Substring(0, 2) == "/{" && placeholders.Count == 1 && upstreamTemplate.Length == postitionOfPlaceHolderClosingBracket + 1)
            {
                return true;
            }

            return false;
        }
        private static bool IsPlaceHolder(string upstreamTemplate, int i)
        {
            return upstreamTemplate[i] == '{';
        }
    }
}

