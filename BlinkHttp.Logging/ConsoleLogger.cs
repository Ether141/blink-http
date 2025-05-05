namespace BlinkHttp.Logging;

internal class ConsoleLogger : ILogSink
{
    internal string? Format { get; set; }

    public void Log(LogMessage logMessage)
    {
        Console.WriteLine(logMessage.GetFormattedMessage(null));
    }
}
