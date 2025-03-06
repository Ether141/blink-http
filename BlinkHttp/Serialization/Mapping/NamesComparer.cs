using System.Text.RegularExpressions;

namespace BlinkHttp.Serialization.Mapping;

internal class NamesComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        string? splitX = NormalizeName(x);
        string? splitY = NormalizeName(y);

        if (splitX == null || splitY == null)
        {
            throw new ArgumentNullException("One or both of given names are empty, or null.");
        }

        return splitX == splitY;
    }

    private static string? NormalizeName(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        string normalized = Regex.Replace(input, @"[^a-zA-Z0-9]+", " ").Trim();

        if (string.IsNullOrEmpty(normalized))
        {
            return null;
        }

        string[] words = [.. normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToLower())];
        return string.Join("", words);
    }

    public int GetHashCode(string obj) => throw new NotImplementedException();
}
