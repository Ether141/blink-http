namespace BlinkHttp.Logging;

internal class ConsoleLogger : IGeneralLogger
{
    private readonly StandardOutputLogger? standardOutputLogger;
    private readonly ExternalOutputLogger? externalOutputLogger;

    public ConsoleLogger(string? format, bool colorful, bool standardOutput, bool externalOutput)
    {
        if (standardOutput)
        {
            standardOutputLogger = new StandardOutputLogger(format, colorful);
        }

        if (externalOutput)
        {
            externalOutputLogger = new ExternalOutputLogger(format);
        }
    }

    public void Log(string message, string? loggerName, LogLevel logLevel)
    {
        standardOutputLogger?.Log(message, loggerName, logLevel);
        externalOutputLogger?.Log(message, loggerName, logLevel);
    }
}
