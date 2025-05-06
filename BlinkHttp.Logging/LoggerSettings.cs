namespace BlinkHttp.Logging;

public class LoggerSettings
{
    internal bool IsConsoleEnabled { get; private set; } = false;
    internal bool IsFileEnabled { get; private set; } = false;

    internal string? ConsoleLogFormat { get; private set; }
    internal string? FileLogFormat { get; private set; }

    internal bool IsTraceEnabled { get; private set; } = true;
    internal bool IsInformationEnabled { get; private set; } = true;
    internal bool IsDebugEnabled { get; private set; } = true;

    internal bool ColorfulConsole { get; private set; } = false;

    internal string? FileLogPath { get; private set; }
    internal string? FileLogFooter { get; private set; }

    public LoggerSettings UseConsole()
    {
        IsConsoleEnabled = true;
        return this;
    }

    public LoggerSettings UseFile()
    {
        IsFileEnabled = true;
        return this;
    }

    public LoggerSettings SetConsoleLogFormat(string format)
    {
        ConsoleLogFormat = format;
        return this;
    }

    public LoggerSettings SetFileLogFormat(string format)
    {
        FileLogFormat = format;
        return this;
    }

    public LoggerSettings SetIsTraceEnabled(bool isTraceEnabled)
    {
        IsTraceEnabled = isTraceEnabled;
        return this;
    }

    public LoggerSettings SetIsInformationEnabled(bool isInformationEnabled)
    {
        IsInformationEnabled = isInformationEnabled;
        return this;
    }

    public LoggerSettings SetIsDebugEnabled(bool isDebugEnabled)
    {
        IsDebugEnabled = isDebugEnabled;
        return this;
    }

    public LoggerSettings EnableColorfulConsole()
    {
        ColorfulConsole = true;
        return this;
    }

    public LoggerSettings SetFileLogPath(string path)
    {
        FileLogPath = path;
        return this;
    }

    public LoggerSettings EnableFileLogFooter(string fileLogFooter)
    {
        FileLogFooter = fileLogFooter;
        return this;
    }
}
