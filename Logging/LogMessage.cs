namespace Logging;

internal class LogMessage
{
    internal string Message { get; }
    internal LogLevel Level { get; }
    internal string? LoggerName { get; }

    public LogMessage(string message, LogLevel level, string? loggerName)
    {
        Message = message;
        Level = level;
        LoggerName = loggerName;
    }

    public string GetFormattedMessage(string? format) => LogFormatter.GetFormattedString(Message, LoggerName, Level, format);
}
