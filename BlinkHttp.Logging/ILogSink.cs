namespace BlinkHttp.Logging;

internal interface ILogSink
{
    void Log(LogMessage logMessage);
}
