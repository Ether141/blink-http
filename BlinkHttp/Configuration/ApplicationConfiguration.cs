using Logging;
using System.Reflection;

namespace BlinkHttp.Configuration;

/// <summary>
/// Allows to load application configuration from .cfg file and then access its properties.
/// </summary>
public class ApplicationConfiguration : IConfiguration
{
    /// <summary>
    /// Default localization of the configuration file in the system. If configuration file is not found, it will be created under this path.
    /// </summary>
    public static string DefaultConfigFilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, DefaultConfigFileName);

    /// <summary>
    /// Path of currently loaded configuration file.
    /// </summary>
    public string? CurrentConfigFilePath { get; private set; }

    /// <summary>
    /// Indicates whether configuration file is loaded correctly.
    /// </summary>
    public bool IsConfigurationLoaded => values != null;

    /// <summary>
    /// Default name of the configuration file.
    /// </summary>
    public const string DefaultConfigFileName = "config.cfg";

    private Dictionary<string, string>? values;
    private readonly ILogger logger = Logger.GetLogger<ApplicationConfiguration>();
    private ConfigurationValuesProvider valuesProvider;

    /// <summary>
    /// Loads configuration file, using <seealso cref="DefaultConfigFilePath"/>.
    /// </summary>
    public void LoadConfiguration() => LoadConfiguration(null);

    /// <summary>
    /// Loads configuration file from given path.
    /// </summary>
    public void LoadConfiguration(string? fileName)
    {
        logger.Debug("== Starting loading configuration");
        EnsureFileExists(fileName);

        logger.Debug(CurrentConfigFilePath!);
        ConfigurationLoader loader = new ConfigurationLoader(logger);
        values = loader.LoadConfiguration(CurrentConfigFilePath!);
        logger.Debug("== Configuration loaded");

        valuesProvider = new ConfigurationValuesProvider(values!, logger);
    }

    /// <summary>
    /// Returns value from configuration with given key. If key does not exist, returns null.
    /// </summary>
    public string? this[string key]
    {
        get
        {
            EnsureConfigurationIsLoaded();
            return valuesProvider.Get(key);
        }
    }

    /// <summary>
    /// Returns value from configuration with given index. If value with given index does not exist, returns null.
    /// </summary>
    public string? this[int index]
    {
        get
        {
            EnsureConfigurationIsLoaded();
            return valuesProvider.Get(index);
        }
    }

    /// <summary>
    /// Returns value from configuration with given key and casts it to the given type. If key does not exist, throws exception.
    /// </summary>
    public T Get<T>(string key)
    {
        EnsureConfigurationIsLoaded();
        return valuesProvider.Get<T>(key);
    }

    /// <summary>
    /// Returns value from configuration with given key as string. If key does not exist, throws exception.
    /// </summary>
    public string Get(string key) => this[key] ?? throw new ApplicationConfigurationException($"Key '{key}' cannot be found.");

    /// <summary>
    /// Returns value from configuration with given key as array of values. If key does not exist, throws exception.
    /// </summary>
    public string[] GetArray(string key) => valuesProvider.GetArray(key);

    /// <summary>
    /// Returns value from configuration with given key as array of values casted to given type. If key does not exist, throws exception.
    /// </summary>
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
