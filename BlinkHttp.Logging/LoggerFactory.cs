namespace BlinkHttp.Logging;

public class LoggerFactory
{
    private static LoggerFactory? factory;

    public static LoggerFactory Instance => factory ?? new LoggerFactory([new VoidLogger()]);

    private readonly ILogSink[] logSinks;
    private readonly Dictionary<string, ILogger> loggers = [];

    private LoggerFactory(ILogSink[] logSinks)
    {
        this.logSinks = logSinks;
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

        logger = new Logger(name, logSinks);
        loggers.Add(name, logger);
        return logger;
    }

    public static LoggerFactory CreateFactory(LoggerSettings settings)
    {
        if (factory == null)
        {
            factory = new LoggerFactory(GetLogSinks(settings));
        }

        return factory;
    }

    private static ILogSink[] GetLogSinks(LoggerSettings settings)
    {
        List<ILogSink> loggers = [];

        if (settings.IsConsoleEnabled)
        {
            loggers.Add(new ConsoleLogger());
        }

        if (settings.IsFileEnabled)
        {

        }

        return [.. loggers];
    }
}
