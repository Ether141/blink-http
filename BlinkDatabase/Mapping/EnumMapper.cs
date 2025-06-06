namespace BlinkDatabase.Mapping;

internal static class EnumMapper
{
    internal static object? TryMap(Type enumType, string? value)
    {
        if (!enumType.IsEnum || string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        string[] parts = value.Split('_', StringSplitOptions.RemoveEmptyEntries);
        string pascalValue = string.Concat(parts.Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1).ToLowerInvariant()));

        return Enum.TryParse(enumType, pascalValue, ignoreCase: false, out object? result) ? result : null;
    }
}
