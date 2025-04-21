namespace Logging;

public class Logger : ILogger
{
    private static readonly Dictionary<string, ILogger> loggersForTypes = [];
    private static readonly HashSet<IGeneralLogger> loggersSet = [];

    private readonly HashSet<IGeneralLogger> loggers;
    private readonly string name;

    private static bool isConfigured = false;

    private Logger(string name, HashSet<IGeneralLogger> loggers)
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
            loggersSet.Add(new ConsoleLogger(loggerSettings.ConsoleLoggerOptions?.MessageFormat));
        }

        if (loggerSettings.IsFileUsed)
        {
            loggersSet.Add(new FileLogger(loggerSettings.FileLoggerOptions?.FilePath, loggerSettings.FileLoggerOptions?.MessageFormat));
        }

        isConfigured = true;
    }

    public static void CleanupLoggers()
    {
        foreach (IGeneralLogger logger in loggersSet)
        {
            if (logger is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        loggersSet.Clear();
    }

    public static ILogger GetLogger<T>() => GetLogger(typeof(T).Name);

    public static ILogger GetLogger(Type forType) => GetLogger(forType.Name);

    public static ILogger GetLogger(string name)
    {
        if (loggersForTypes.TryGetValue(name, out ILogger? value))
        {
            return value;
        }

        value = new Logger(name, loggersSet);
        loggersForTypes.Add(name, value);
        return value;
    }

    public void Debug(object? message) => CallAllLoggers(message, LogLevel.Debug);

    public void Info(object? message) => CallAllLoggers(message, LogLevel.Info);

    public void Warning(object? message) => CallAllLoggers(message, LogLevel.Warning);

    public void Error(object? message) => CallAllLoggers(message, LogLevel.Error);

    public void Critical(object? message) => CallAllLoggers(message, LogLevel.Critical);

    private void CallAllLoggers(object? message, LogLevel level)
    {
        string text = message is string ? (string)message : (message == null ? string.Empty : message.ToString() ?? string.Empty);
        LogMessage msg = new LogMessage(text, level, name);

        foreach (IGeneralLogger logger in loggers)
        {
            logger.Log(msg);
        }
    }
}
