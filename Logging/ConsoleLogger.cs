namespace Logging;

internal class ConsoleLogger : IGeneralLogger
{
    private readonly string? format;

    public ConsoleLogger(string? format)
    {
        this.format = format;
    }

    public void Log(LogMessage message) => Write(message.GetFormattedMessage(format));

    private void Write(string message) => Console.WriteLine(message);
}
