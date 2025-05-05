namespace BlinkHttp.Logging;

internal class VoidLogger : ILogSink
{
    public void Log(LogMessage logMessage) { }
}
