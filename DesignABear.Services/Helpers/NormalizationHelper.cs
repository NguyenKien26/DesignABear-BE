using System.Globalization;
using System.Text;

namespace DesignABear.Services.Helpers
{
    /// <summary>
    /// Utility for normalizing Vietnamese text — removes diacritics and lowercases.
    /// Useful for search/filter operations.
    /// </summary>
    public static class NormalizationHelper
    {
        /// <summary>
        /// Remove Vietnamese diacritics and return lowercase ASCII text.
        /// E.g. "Gấu Bông" → "gau bong"
        /// </summary>
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var ch in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }

        /// <summary>
        /// Trim and collapse internal whitespace in a string.
        /// </summary>
        public static string Normalize(string? text) =>
            string.IsNullOrWhiteSpace(text)
                ? string.Empty
                : string.Join(' ', text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
