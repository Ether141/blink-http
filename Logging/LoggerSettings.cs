namespace Logging;

/// <summary>
/// Represents the settings for the logger.
/// </summary>
public class LoggerSettings
{
    internal bool IsConsoleUsed { get; private set; }
    internal bool IsFileUsed { get; private set; }
    internal string? Format { get; private set; }
    internal LoggerOptions? ConsoleLoggerOptions { get; private set; }
    internal FileLoggerOptions? FileLoggerOptions { get; private set; }

    /// <summary>
    /// Configures the logger to use the console.
    /// </summary>
    public LoggerSettings UseConsole()
    {
        ConsoleLoggerOptions = new LoggerOptions();
        IsConsoleUsed = true;
        return this;
    }

    /// <summary>
    /// Configures the logger to use a file.
    /// </summary>
    public LoggerSettings UseFile()
    {
        IsFileUsed = true;
        return this;
    }

    /// <summary>
    /// Configures the logger to use a file with the specified options.
    /// </summary>
    public LoggerSettings UseFile(Action<FileLoggerOptions> options)
    {
        FileLoggerOptions = new FileLoggerOptions();
        options?.Invoke(FileLoggerOptions);
        IsFileUsed = true;
        return this;
    }

    /// <summary>
    /// Sets the message format for the logger.
    /// </summary>
    /// <remarks>%d - short date, %D - long date, %t - short time only, %T long time only, %name logger name, %message log message, %level log level. After each parameter, you can add padding, e.g. %log:10, %log:-7</remarks>
    public LoggerSettings SetMessageFormat(string format)
    {
        ConsoleLoggerOptions?.SetMessageFormat(format);
        FileLoggerOptions?.SetMessageFormat(format);

        return this;
    }
}
