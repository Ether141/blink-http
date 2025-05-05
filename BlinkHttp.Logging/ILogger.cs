namespace BlinkHttp.Logging;

public interface ILogger
{
    string Name { get; }

    void Log(LogMessage logMessage);
    void Debug(object message);
    void Info(object message);
    void Warning(object message);
    void Error(object message);
    void Critical(object message);
}
