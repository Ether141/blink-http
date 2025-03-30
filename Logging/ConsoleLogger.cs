namespace Logging;

internal class ConsoleLogger : IGeneralLogger
{
    private readonly string? format;

    public ConsoleLogger(string? format)
    {
        this.format = format;
    }

    public void Log(string message, string? loggerName, LogLevel logLevel) => Write(message, loggerName, logLevel);

    private void Write(string message, string? loggerName, LogLevel logLevel)
    {
        message = LoggingHelper.GetFormattedString(message, loggerName, logLevel, format);
        Console.WriteLine(message);
    }
}
