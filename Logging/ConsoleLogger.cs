namespace Logging;

internal class ConsoleLogger : IGeneralLogger
{
    private readonly StandardOutputLogger? standardOutputLogger;

    public ConsoleLogger(string? format, bool colorful, bool standardOutput)
    {
        if (standardOutput)
        {
            standardOutputLogger = new StandardOutputLogger(format, colorful);
        }
    }

    public void Log(string message, string? loggerName, LogLevel logLevel)
    {
        standardOutputLogger?.Log(message, loggerName, logLevel);
    }
}
