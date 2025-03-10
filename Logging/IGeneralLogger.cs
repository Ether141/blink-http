namespace Logging;

internal interface IGeneralLogger
{
    public void Log(string message, string? loggerName, LogLevel logLevel);
}
