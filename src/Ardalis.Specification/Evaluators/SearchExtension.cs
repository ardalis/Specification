using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ardalis.Specification;

public static class SearchExtension
{
    public static bool Like(this string input, string pattern)
    {
        try
        {
            return SqlLike(input, pattern);
        }
        catch (Exception ex)
        {
            throw new InvalidSearchPatternException(pattern, ex);
        }
    }

    private static bool SqlLike(this string input, string pattern)
    {
        // Escape special regex characters, excluding those handled separately
        var regexPattern = Regex.Escape(pattern)
            .Replace("%", ".*")     // Translate SQL LIKE wildcard '%' to regex '.*'
            .Replace("_", ".")      // Translate SQL LIKE wildcard '_' to regex '.'
            .Replace(@"\[", "[")    // Unescape '[' as it's used for character classes/ranges
            .Replace(@"\^", "^");   // Unescape '^' as it can be used for negation in character classes

        // Ensure the pattern matches the entire string
        regexPattern = "^" + regexPattern + "$";
        var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(input);
    }

    // This C# implementation of SQL Like operator is based on the following SO post https://stackoverflow.com/a/8583383/10577116
    // It covers almost all of the scenarios, and it's faster than regex based implementations.
    // It may fail/throw in some very specific and edge cases, hence, wrap it in try/catch.
    // UPDATE: it returns incorrect results for some obvious cases.
    // More details in this issue https://github.com/ardalis/Specification/issues/390
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
}
