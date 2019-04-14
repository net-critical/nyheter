using System;
using System.Text.RegularExpressions;

namespace NewsProvider.Utils
{
    /// <summary>
    /// Utilities to provide som functionality for html related strings.
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Extract first image found in html string.
        /// </summary>
        /// <param name="html">Raw HTML string.</param>
        /// <returns>Image url.</returns>
        public static string ExtractImage(this string html)
        {
            const string pattern = @"<img\b[^\<\>]+?\bsrc\s*=\s*[""'](?<L>.+?)[""'][^\<\>]*?\>";

            foreach (Match match in Regex.Matches(html, pattern, RegexOptions.IgnoreCase))
            {
                return match.Groups["L"].Value;
            }

            return null;
        }

        /// <summary>
        /// Convert raw HTML to text (remove tags).
        /// </summary>
        /// <param name="html">Raw HTML string.</param>
        /// <returns>Text without HTML tags.</returns>
        public static string ExtractText(this string html)
        {
            return Regex.Replace(html, "<.*?>", String.Empty);
        }

        /// <summary>
        /// Generate hyphenated slug suitable to use as uri param.
        /// Simple transliteration works only with swedish letters.
        /// </summary>
        /// <param name="str">Text to slugify.</param>
        /// <returns>Slug string.</returns>
        public static string GenerateSlug(this string str)
        {
            // Simple swedish transliteration.
            str = str.Replace('ö', 'o')
                 .Replace('Ö', 'O')
                 .Replace('ä', 'a')
                 .Replace('Ä', 'A')
                 .Replace('å', 'a')
                 .Replace('Å', 'A');

            // Remove invalid characters.
            str = Regex.Replace(str.ToLower(), @"[^a-z0-9\s-]", "");

            // Remove multiple spaces.
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Replace spaces with hyphens.
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        /// <summary>
        /// Convert date stringto DateTime.
        /// </summary>
        /// <param name="date">Date string to parse.</param>
        /// <returns>Return date parsed or today's date on error.</returns>
        public static DateTime ParseDate(this string date)
        {
            if (DateTime.TryParse(date, out DateTime result))
            {
                return result;
            }

            return DateTime.Now;
        }
    }
}
