namespace BlinkHttp.Logging;

internal class Logger : ILogger
{
    public string Name { get; }

    private readonly ILogSink[] loggers;

    public Logger(string name, ILogSink[] loggers)
    {
        this.Name = name;
        this.loggers = loggers;
    }

    public void Critical(object message) => Log(GetMessage(message, LogLevel.Critical));

    public void Debug(object message) => Log(GetMessage(message, LogLevel.Debug));

    public void Error(object message) => Log(GetMessage(message, LogLevel.Error));

    public void Info(object message) => Log(GetMessage(message, LogLevel.Information));

    public void Warning(object message) => Log(GetMessage(message, LogLevel.Warning));

    public void Log(LogMessage logMessage)
    {
        foreach (ILogSink logger in loggers)
        {
            logger.Log(logMessage);
        }
    }

    private LogMessage GetMessage(object message, LogLevel level) => new LogMessage(message, level) { LoggerName = Name };
}
