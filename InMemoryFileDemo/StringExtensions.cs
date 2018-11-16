using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InMemoryFileDemo
{
    public static class StringExtensions
    {
        /// <exception cref = "ArgumentNullException">When <c>toTrim</c> is null.</exception>
        /// <exception cref = "ArgumentException"><c>ArgumentException</c>.</exception>
        public static string WithoutSuffix(this string toTrim, string suffix)
        {
            if (toTrim == null)
                throw new ArgumentNullException("toTrim", "String to trim the end from was null");

            if (string.IsNullOrEmpty(suffix))
                return toTrim;

            if (!toTrim.EndsWith(suffix))
                throw new ArgumentException(
                    String.Format("The: {0} string does not end with: {1}, and so was the intention of the code.", toTrim, suffix));

            return toTrim.Substring(0, toTrim.LastIndexOf(suffix, StringComparison.Ordinal));
        }
        public static bool Contains(this string source, string pattern, StringComparison comparison)
        {
            return source.IndexOf(pattern, comparison) >= 0;
        }

        public static bool ContainsAny(this string source, IEnumerable<string> patterns, StringComparison comparison)
        {
            return patterns.Any(pattern => source.Contains(pattern, comparison));
        }

        public static bool ContainsAll(this string source, IEnumerable<string> patterns, StringComparison comparison)
        {
            return patterns.All(pattern => source.Contains(pattern, comparison));
        }

        public static bool StartsWithAny(this string source, params string[] patterns)
        {
            return patterns.Any(source.StartsWith);
        }

        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static string AsNullIfWhitespace(this string source)
        {
            if (source.IsNullOrWhitespace())
                return null;

            return source;
        }

        public static bool IsNotNullNorEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        public static string Mask(this string source, int visibleCharsCount = 0)
        {
            if (source.IsNullOrEmpty() || source.Length <= visibleCharsCount)
                return source;

            int maskedSourceElementsCount = source.Length - visibleCharsCount;
            var maskedSourcePart = new string('*', maskedSourceElementsCount);
            string visibleSourcePart = source.Substring(maskedSourceElementsCount);

            return maskedSourcePart + visibleSourcePart;
        }



        public static string RemoveExtraWhiteSpaces(this string source)
        {
            if (source == null)
                return null;

            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            var regex = new Regex(@"\s+");
            return regex.Replace(source.Trim(), " ");
        }

        public static string RemoveWhiteSpaces(this string source)
        {
            if (source.IsNullOrEmpty())
                return source;

            return Regex.Replace(source, @"\s+", string.Empty);
        }

        public static bool MatchesRegex(this string source, string regexPattern)
        {
            return Regex.IsMatch(source, regexPattern);
        }

        public static string SubstringUpTo(this string source, int upToChars)
        {
            if (source.Length > upToChars)
                return source.Substring(0, upToChars);

            return source;
        }

        public static string FirstLetterToUpper(this string source)
        {
            if (source == null)
                return null;

            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return source.First().ToString().ToUpper() + source.Substring(1);
        }

        public static string GetInitials(this string fullName)
        {
            if (fullName == null)
                return null;

            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var punctuation = fullName.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = fullName.Split().Select(x => x.Trim(punctuation)).Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();

            if (words.Length == 0)
                return string.Empty;

            var result = Char.ToUpper(words.First()[0]).ToString();

            if (words.Length > 1)
                result += Char.ToUpper(words.Last()[0]);

            return result;
        }
    }
}
