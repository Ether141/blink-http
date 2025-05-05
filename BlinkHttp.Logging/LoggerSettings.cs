namespace BlinkHttp.Logging;

public class LoggerSettings
{
    internal bool IsConsoleEnabled { get; private set; }
    internal bool IsFileEnabled { get; private set; }

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
}
