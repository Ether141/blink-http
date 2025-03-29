namespace Logging;

public class Logger : ILogger
{
    private static readonly Dictionary<string, ILogger> loggersForTypes = [];
    private static readonly HashSet<IGeneralLogger> loggersSet = [];

    private readonly IGeneralLogger[] loggers;
    private readonly string name;

    private static bool isConfigured = false;

    private Logger(string name, IGeneralLogger[] loggers)
    {
        this.name = name;
        this.loggers = loggers;
    }

    public static void Configure(Action<LoggerSettings> settings)
    {
        if (isConfigured)
        {
            return;
        }

        LoggerSettings loggerSettings = new LoggerSettings();
        settings?.Invoke(loggerSettings);

        if (loggerSettings.IsConsoleUsed)
        {
            loggersSet.Add(new ConsoleLogger(loggerSettings.ConsoleLoggerOptions?.MessageFormat, loggerSettings.ConsoleLoggerOptions?.ColorfulConsole ?? false));
        }

        if (loggerSettings.IsFileUsed)
        {
            loggersSet.Add(new FileLogger(loggerSettings.FileLoggerOptions?.FilePath, loggerSettings.FileLoggerOptions?.MessageFormat));
        }

        isConfigured = true;
    }

    public static ILogger GetLogger<T>() => GetLogger(typeof(T));

    public static ILogger GetLogger(Type forType) => GetLogger(forType.Name);

    public static ILogger GetLogger(string name)
    {
        if (loggersForTypes.TryGetValue(name, out ILogger? value))
        {
            return value;
        }

        value = new Logger(name, GetLoggersSet());
        loggersForTypes.Add(name, value);
        return value;
    }

    private static IGeneralLogger[] GetLoggersSet() => [.. loggersSet];

    public void Debug(string message) => CallAllLoggers(message, LogLevel.Debug);

    public void Info(string message) => CallAllLoggers(message, LogLevel.Info);

    public void Warning(string message) => CallAllLoggers(message, LogLevel.Warning);

    public void Error(string message) => CallAllLoggers(message, LogLevel.Error);

    public void Critical(string message) => CallAllLoggers(message, LogLevel.Critical);

    private void CallAllLoggers(string message, LogLevel level)
    {
        foreach (IGeneralLogger logger in loggers)
        {
            logger.Log(message, name, level);
        }
    }
}
