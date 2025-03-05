namespace BlinkHttp.Logging;

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Critical
}

public static class LogLevelUtil
{
    public static string GetString(this LogLevel level) => level switch
    {
        LogLevel.Warning => "WARN",
        LogLevel.Error => "ERROR",
        LogLevel.Info => "INFO",
        LogLevel.Critical => "CRIT",
        _ => "DEBUG"
    };
}