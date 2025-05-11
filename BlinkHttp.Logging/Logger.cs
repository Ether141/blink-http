namespace BlinkHttp.Logging;

internal class Logger : ILogger
{
    public string Name { get; }

    private readonly LoggerSettings settings;
    private readonly ILogSink[] loggers;

    private string? currentScope;

    public Logger(string name, LoggerSettings settings, ILogSink[] loggers)
    {
        Name = name;
        this.settings = settings;
        this.loggers = loggers;
    }

    public void Critical(object? message) => Log(GetMessage(message, LogLevel.Critical));

    public void Debug(object? message) => Log(GetMessage(message, LogLevel.Debug));

    public void Error(object? message) => Log(GetMessage(message, LogLevel.Error));

    public void Info(object? message) => Log(GetMessage(message, LogLevel.Information));

    public void Warning(object? message) => Log(GetMessage(message, LogLevel.Warning));

    public void Log(LogMessage logMessage)
    {
        if ((logMessage.LogLevel == LogLevel.Trace && !settings.IsTraceEnabled) || (logMessage.LogLevel == LogLevel.Information && !settings.IsInformationEnabled) || 
            (logMessage.LogLevel == LogLevel.Debug && !settings.IsDebugEnabled))
        {
            return;
        }

        foreach (ILogSink logger in loggers)
        {
            logger.Log(logMessage);
        }
    }

    private LogMessage GetMessage(object? message, LogLevel level) => new LogMessage(message ?? string.Empty, level) { LoggerName = Name, Scope = currentScope };

    public void BeginScope(string scopeName) => currentScope = scopeName;

    public void EndScope() => currentScope = null;
}
