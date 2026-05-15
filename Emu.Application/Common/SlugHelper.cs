using System.Text;
using System.Text.RegularExpressions;

namespace Application.Common;

public static partial class SlugHelper
{
    public static string Generate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalized = value.Trim().ToLowerInvariant();

        normalized = RemoveDiacritics(normalized);

        normalized = InvalidCharactersRegex().Replace(normalized, "");

        normalized = SpacesRegex().Replace(normalized, "-");

        normalized = MultipleHyphensRegex().Replace(normalized, "-");

        return normalized.Trim('-');
    }

    private static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var character in normalized)
        {
            var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character);

            if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex InvalidCharactersRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpacesRegex();

    [GeneratedRegex(@"-+")]
    private static partial Regex MultipleHyphensRegex();
}
