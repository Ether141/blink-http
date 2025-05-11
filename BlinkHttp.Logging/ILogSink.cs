namespace BlinkHttp.Logging;

internal interface ILogSink
{
    string? Format { get; set; }

    void Log(LogMessage logMessage);
}
