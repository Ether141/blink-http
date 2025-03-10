using Logging;

namespace BlinkHttp.Configuration;

internal class ConfigurationLoader
{
    private string? currentSection = null;
    private ILogger? logger = null;

    public ConfigurationLoader(ILogger? logger)
    {
        this.logger = logger;
    }

    internal Dictionary<string, string> LoadConfiguration(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        Dictionary<string, string> values = [];

        foreach (string line in lines)
        {
            (string? key, string? value) = ParseLine(line);

            if (key != null && value != null)
            {
                if (!values.TryAdd(key, value))
                {
                    LogError($"Configuration file already has key: {key}");
                }
            }
        }

        return values;
    }

    private (string? key, string? value) ParseLine(string line)
    {
        line = line.Trim();

        if (string.IsNullOrWhiteSpace(line))
        {
            currentSection = null;
            return (null, null);
        }

        if (line.StartsWith('#'))
        {
            return (null, null);
        }

        if (line.Contains('#'))
        {
            line = line[..line.IndexOf('#')];
        }

        if (line.StartsWith('[') && line.EndsWith(']'))
        {
            currentSection = line[1..^1];
            return (null, null);
        }

        if (!line.Contains('='))
        {
            LogError($"line does not contain '=' delimeter ({line})");
            return (null, null);
        }

        string[] parts = line.Split('=');

        if (parts.Length > 2)
        {
            LogError($"line cannot contain more than one '=' delimeter ({line})");
            return (null, null);
        }

        string key = parts[0].Trim();
        string value = parts[^1].Trim();

        if (currentSection != null)
        {
            key = $"{currentSection}:{key}";
        }

        return (key, value);
    }

    private void LogError(string message) => logger?.Error($"Error during reading configuration file: {message}");
}
