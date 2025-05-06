namespace BlinkHttp.Logging;

public class LoggerFactory
{
    private static LoggerFactory? factory;

    internal static LoggerFactory Instance => factory ?? new LoggerFactory([], new LoggerSettings());

    private readonly ILogSink[] logSinks;
    private readonly LoggerSettings settings;
    private readonly Dictionary<string, ILogger> loggers = [];

    private LoggerFactory(ILogSink[] logSinks, LoggerSettings settings)
    {
        this.logSinks = logSinks;
        this.settings = settings;
    }

    public static ILogger Create<T>() => Instance.InternalCreate<T>();

    public static ILogger Create(Type type) => Instance.InternalCreate(type);

    public static ILogger Create(string name) => Instance.InternalCreate(name);

    private ILogger InternalCreate<T>() => InternalCreate(typeof(T));

    private ILogger InternalCreate(Type type) => InternalCreate(type.Name);

    private ILogger InternalCreate(string name)
    {
        ILogger? logger = loggers.FirstOrDefault(l => l.Key == name).Value;

        if (logger != null)
        {
            return logger;
        }

        logger = new Logger(name, settings, logSinks);
        loggers.Add(name, logger);
        return logger;
    }

    public static LoggerFactory CreateFactory(LoggerSettings settings)
    {
        if (factory == null)
        {
            factory = new LoggerFactory(GetLogSinks(settings), settings);
        }

        return factory;
    }

    public static void Clean()
    {
        if (factory == null)
        {
            return;
        }

        (factory.logSinks.FirstOrDefault(l => l.GetType() == typeof(FileLogger)) as FileLogger)?.Dispose();
    }

    private static ILogSink[] GetLogSinks(LoggerSettings settings)
    {
        List<ILogSink> loggers = [];

        if (settings.IsConsoleEnabled)
        {
            loggers.Add(new ConsoleLogger() { Format = settings.ConsoleLogFormat, Colorful = settings.ColorfulConsole });
        }

        if (settings.IsFileEnabled)
        {
            loggers.Add(new FileLogger(settings.FileLogPath) { Format = settings.FileLogFormat, FileLogFooter = settings.FileLogFooter });
        }

        return [.. loggers];
    }
}
