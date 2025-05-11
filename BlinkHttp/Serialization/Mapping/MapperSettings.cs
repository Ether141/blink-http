namespace BlinkHttp.Serialization.Mapping;

internal class MapperSettings
{
    internal IEqualityComparer<string> NamesComparer { get; }

    public MapperSettings(IEqualityComparer<string> namesComparer)
    {
        NamesComparer = namesComparer;
    }

    internal static MapperSettings Default => new MapperSettings(new NamesComparer());
}
