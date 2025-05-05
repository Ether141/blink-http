namespace BlinkHttp.Logging;

public class LogMessage
{
    public string Message { get; }
    public string? LoggerName { get; init; }
    public LogLevel LogLevel { get; }
    public DateTime Timestamp { get; }

    public LogMessage(object message, LogLevel logLevel) : this(message, logLevel, DateTime.Now) { }

    public LogMessage(object message, LogLevel logLevel, DateTime timestamp)
    {
        Message = GetString(message);
        LogLevel = logLevel;
        Timestamp = timestamp;
    }

    internal string GetFormattedMessage(string? format) => LogFormatter.GetFormattedString(Message, LoggerName, LogLevel, format ?? LogFormatter.DefaultFormat);

    private static string GetString(object message) => message is string s ? s : message.ToString() ?? "";
}
