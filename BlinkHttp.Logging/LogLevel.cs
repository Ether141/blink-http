namespace BlinkHttp.Logging;

public enum LogLevel
{
    Trace,
    Debug,
    Information,
    Warning,
    Error,
    Critical
}

internal static class LogLevelExtension
{
    public static string GetString(this LogLevel level) => level switch
    {
        LogLevel.Trace => "TRACE",
        LogLevel.Warning => "WARN",
        LogLevel.Error => "ERROR",
        LogLevel.Information => "INFO",
        LogLevel.Critical => "CRIT",
        _ => "DEBUG"
    };
}