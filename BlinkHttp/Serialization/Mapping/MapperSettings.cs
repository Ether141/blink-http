namespace BlinkHttp.Serialization.Mapping;

internal class MapperSettings
{
    internal IEqualityComparer<string> NamesComparer { get; set; }

    internal static MapperSettings Default => new MapperSettings()
    {
        NamesComparer = new NamesComparer()
    };
}
