namespace BlinkHttp.Logging;

public class LoggerSettings
{
    internal bool IsConsoleUsed { get; private set; }
    internal bool IsFileUsed { get; private set; }
    internal string? Format { get; private set; }
    internal ConsoleLoggerOptions? ConsoleLoggerOptions { get; private set; }
    internal FileLoggerOptions? FileLoggerOptions { get; private set; }

    public LoggerSettings UseConsole()
    {
        IsConsoleUsed = true;
        return this;
    }

    public LoggerSettings UseConsole(Action<ConsoleLoggerOptions> options)
    {
        ConsoleLoggerOptions = new ConsoleLoggerOptions();
        options?.Invoke(ConsoleLoggerOptions);
        IsConsoleUsed = true;
        return this;
    }

    public LoggerSettings UseFile()
    {
        IsFileUsed = true;
        return this;
    }

    public LoggerSettings UseFile(Action<FileLoggerOptions> options)
    {
        FileLoggerOptions = new FileLoggerOptions();
        options?.Invoke(FileLoggerOptions);
        IsFileUsed = true;
        return this;
    }

    public LoggerSettings SetMessageFormat(string format)
    {
        if (ConsoleLoggerOptions != null && ConsoleLoggerOptions.MessageFormat == null)
        {
            ConsoleLoggerOptions.SetMessageFormat(format);
        }

        if (FileLoggerOptions != null && FileLoggerOptions.MessageFormat == null)
        {
            FileLoggerOptions.SetMessageFormat(format);
        }

        return this;
    }
}
