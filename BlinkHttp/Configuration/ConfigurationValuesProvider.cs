using Logging;

namespace BlinkHttp.Configuration;

internal class ConfigurationValuesProvider
{
    private readonly Dictionary<string, string> values;
    private readonly ILogger logger;

    internal ConfigurationValuesProvider(Dictionary<string, string> values, ILogger logger)
    {
        this.values = values;
        this.logger = logger;
    }

    public string[] GetArray(string key)
    {
        if (TryGetValue(key, out string? value))
        {
            string[] vals = value!.Trim(',').Split(',').Select(v => v.Trim()).ToArray();
            return vals;
        }
        else
        {
            throw new ApplicationConfigurationException($"Key '{key}' cannot be found.");
        }
    }

    public T[] GetArray<T>(string key) => ConfigurationCaster.ArrayToType<T>(GetArray(key));

    public T? GetOrDefault<T>(string key)
    {
        try
        {
            return Get<T>(key);
        }
        catch
        {
            logger.Error($"Unable to find or cast value with key: {key}");
            return default;
        }
    }

    public string? Get(string key) => TryGetValue(key, out string? value) ? value : null;

    public string? Get(int index) => index < 0 || index >= values.Count ? null : values.ElementAt(index).Value;

    public T Get<T>(string key)
    {
        if (TryGetValue(key, out string? value))
        {
            return ConfigurationCaster.ToType<T>(value!);
        }
        else
        {
            throw new ApplicationConfigurationException($"Key '{key}' cannot be found.");
        }
    }

    private bool TryGetValue(string key, out string? value)
    {
        if (key.Contains(':'))
        {
            return values.TryGetValue(key, out value);
        }

        bool valueFound = values.TryGetValue(key, out value);

        if (valueFound)
        {
            return true;
        }

        Func<KeyValuePair<string, string>, bool> predicate = pair => key == pair.Key[(pair.Key.IndexOf(':') + 1)..];

        if (values.Count(predicate) == 1)
        {
            value = values.First(predicate).Value;
            return true;
        }

        value = null;
        return false;
    }
}
