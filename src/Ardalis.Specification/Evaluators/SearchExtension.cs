using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Ardalis.Specification;

public static class SearchExtension
{
    private static readonly RegexCache _regexCache = new();

    private static Regex BuildRegex(string pattern)
    {
        // Escape special regex characters, excluding those handled separately
        var regexPattern = Regex
            .Escape(pattern)
            .Replace("%", ".*")     // Translate SQL LIKE wildcard '%' to regex '.*'
            .Replace("_", ".")      // Translate SQL LIKE wildcard '_' to regex '.'
            .Replace(@"\[", "[")    // Unescape '[' as it's used for character classes/ranges
            .Replace(@"\^", "^");   // Unescape '^' as it can be used for negation in character classes

        // Ensure the pattern matches the entire string
        regexPattern = "^" + regexPattern + "$";
        var regex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        return regex;
    }

    public static bool Like(this string input, string pattern)
    {
        try
        {
            // The pattern is dynamic and arbitrary, the consumer might even compose it by an end-user input.
            // We can not cache all Regex objects, but at least we can try to reuse the most "recent" ones. We'll cache 10 of them.
            // This might improve the performance within the same closed loop for the in-memory evaluator and validator.

            var regex = _regexCache.GetOrAdd(pattern, BuildRegex);
            return regex.IsMatch(input);
        }
        catch (Exception ex)
        {
            throw new InvalidSearchPatternException(pattern, ex);
        }
    }

    private class RegexCache
    {
        private const int MAX_SIZE = 10;
        private readonly ConcurrentDictionary<string, Regex> _dictionary = new();

        public Regex GetOrAdd(string key, Func<string, Regex> valueFactory)
        {
            if (_dictionary.TryGetValue(key, out var regex))
                return regex;

            // It might happen we end up with more items than max (concurrency), but we won't be too strict.
            // We're just trying to avoid indefinite growth.
            for (int i = _dictionary.Count - MAX_SIZE; i >= 0; i--)
            {
                // Avoid being smart, just remove sequentially from the start.
                var firstKey = _dictionary.Keys.FirstOrDefault();
                if (firstKey is not null)
                {
                    _dictionary.TryRemove(firstKey, out _);
                }

            }

            var newRegex = valueFactory(key);
            _dictionary.TryAdd(key, newRegex);
            return newRegex;
        }
    }

#pragma warning disable IDE0051 // Remove unused private members
    // This C# implementation of SQL Like operator is based on the following SO post https://stackoverflow.com/a/8583383/10577116
    // It covers almost all of the scenarios, and it's faster than regex based implementations.
    // It may fail/throw in some very specific and edge cases, hence, wrap it in try/catch.
    // UPDATE: it returns incorrect results for some obvious cases.
    [ExcludeFromCodeCoverage] // Dead code. Keeping it just as a reference
    private static bool SqlLikeOption2(string str, string pattern)
    {
        var isMatch = true;
        var isWildCardOn = false;
        var isCharWildCardOn = false;
        var isCharSetOn = false;
        var isNotCharSetOn = false;
        var lastWildCard = -1;
        var patternIndex = 0;
        var set = new List<char>();
        var p = '\0';
        bool endOfPattern;

        for (var i = 0; i < str.Length; i++)
        {
            var c = str[i];
            endOfPattern = (patternIndex >= pattern.Length);
            if (!endOfPattern)
            {
                p = pattern[patternIndex];

                if (!isWildCardOn && p == '%')
                {
                    lastWildCard = patternIndex;
                    isWildCardOn = true;
                    while (patternIndex < pattern.Length &&
                        pattern[patternIndex] == '%')
                    {
                        patternIndex++;
                    }
                    p = patternIndex >= pattern.Length ? '\0' : pattern[patternIndex];
                }
                else if (p == '_')
                {
                    isCharWildCardOn = true;
                    patternIndex++;
                }
                else if (p == '[')
                {
                    if (pattern[++patternIndex] == '^')
                    {
                        isNotCharSetOn = true;
                        patternIndex++;
                    }
                    else isCharSetOn = true;

                    set.Clear();
                    if (pattern[patternIndex + 1] == '-' && pattern[patternIndex + 3] == ']')
                    {
                        var start = char.ToUpper(pattern[patternIndex]);
                        patternIndex += 2;
                        var end = char.ToUpper(pattern[patternIndex]);
                        if (start <= end)
                        {
                            for (var ci = start; ci <= end; ci++)
                            {
                                set.Add(ci);
                            }
                        }
                        patternIndex++;
                    }

                    while (patternIndex < pattern.Length &&
                        pattern[patternIndex] != ']')
                    {
                        set.Add(pattern[patternIndex]);
                        patternIndex++;
                    }
                    patternIndex++;
                }
            }

            if (isWildCardOn)
            {
                if (char.ToUpper(c) == char.ToUpper(p))
                {
                    isWildCardOn = false;
                    patternIndex++;
                }
            }
            else if (isCharWildCardOn)
            {
                isCharWildCardOn = false;
            }
            else if (isCharSetOn || isNotCharSetOn)
            {
                var charMatch = (set.Contains(char.ToUpper(c)));
                if ((isNotCharSetOn && charMatch) || (isCharSetOn && !charMatch))
                {
                    if (lastWildCard >= 0) patternIndex = lastWildCard;
                    else
                    {
                        isMatch = false;
                        break;
                    }
                }
                isNotCharSetOn = isCharSetOn = false;
            }
            else
            {
                if (char.ToUpper(c) == char.ToUpper(p))
                {
                    patternIndex++;
                }
                else
                {
                    if (lastWildCard >= 0) patternIndex = lastWildCard;
                    else
                    {
                        isMatch = false;
                        break;
                    }
                }
            }
        }
        endOfPattern = (patternIndex >= pattern.Length);

        if (isMatch && !endOfPattern)
        {
            var isOnlyWildCards = true;
            for (var i = patternIndex; i < pattern.Length; i++)
            {
                if (pattern[i] != '%')
                {
                    isOnlyWildCards = false;
                    break;
                }
            }
            if (isOnlyWildCards) endOfPattern = true;
        }
        return isMatch && endOfPattern;
    }
#pragma warning restore IDE0051
}
