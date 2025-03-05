namespace BlinkHttp.Logging;

public abstract class LoggerOptions
{
    internal string? MessageFormat { get; private set; }

    public LoggerOptions SetMessageFormat(string messageFormat)
    {
        MessageFormat = messageFormat;
        return this;
    }
}
