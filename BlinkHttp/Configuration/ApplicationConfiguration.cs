using BlinkHttp.Logging;

namespace BlinkHttp.Configuration;

public class ApplicationConfiguration : IConfiguration
{
    public static string DefaultConfigFilePath => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, DefaultConfigFileName);

    public string? CurrentConfigFilePath { get; private set; }
    public bool IsConfigurationLoaded => values != null;

    public const string DefaultConfigFileName = "config.cfg";

    private Dictionary<string, string>? values;
    private readonly ILogger logger = Logger.GetLogger(typeof(ApplicationConfiguration));
    private ConfigurationValuesProvider valuesProvider;

    public void LoadConfiguration() => LoadConfiguration(null);

    public void LoadConfiguration(string? fileName)
    {
        logger.Debug("== Starting loading configuration");
        EnsureFileExists(fileName);

        ConfigurationLoader loader = new ConfigurationLoader(logger);
        values = loader.LoadConfiguration(CurrentConfigFilePath!);
        logger.Debug("== Configuration loaded");

        valuesProvider = new ConfigurationValuesProvider(values!, logger);
    }

    public string? this[string key]
    {
        get
        {
            EnsureConfigurationIsLoaded();
            return valuesProvider.Get(key);
        }
    }

    public string? this[int index]
    {
        get
        {
            EnsureConfigurationIsLoaded();
            return valuesProvider.Get(index);
        }
    }

    public T Get<T>(string key)
    {
        EnsureConfigurationIsLoaded();
        return valuesProvider.Get<T>(key);
    }

    public string Get(string key) => this[key] ?? throw new ApplicationConfigurationException($"Key '{key}' cannot be found.");

    public string[] GetArray(string key) => valuesProvider.GetArray(key);

    public T[] GetArray<T>(string key)
    {
        EnsureConfigurationIsLoaded();
        return valuesProvider.GetArray<T>(key);
    }

    private void EnsureFileExists(string? fileName)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (File.Exists(fileName))
            {
                CurrentConfigFilePath = fileName;
                return;
            }
            else if (!TryCreateFile(fileName))
            {
                CreateDefaultFile();
                return;
            }
        }

        CreateDefaultFile();

        if (CurrentConfigFilePath == null)
        {
            throw new ApplicationConfigurationException("Unable to create new configuration file.");
        }
    }

    private void CreateDefaultFile() => TryCreateFile(DefaultConfigFilePath);

    private bool TryCreateFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                using var _ = File.Create(filePath);
                logger.Debug($"Created new configuration file: {filePath}");
            }

            CurrentConfigFilePath = filePath;
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Unable to create configuration file ({filePath}): {ex.Message}");
            return false;
        }
    }

    private void EnsureConfigurationIsLoaded()
    {
        if (!IsConfigurationLoaded)
        {
            throw new ApplicationConfigurationException("Configuration is not loaded! Use LoadConfiguration() method first.");
        }
    }
}
