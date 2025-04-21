namespace Logging;

public interface ILogger
{
    void Debug(object? message);
    void Info(object? message);
    void Warning(object? message);
    void Error(object? message);
    void Critical(object? message);
}
