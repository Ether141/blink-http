namespace Logging;

public class ConsoleLoggerOptions : LoggerOptions
{
    internal bool ColorfulConsole { get; private set; }

    public ConsoleLoggerOptions EnableColorfulConsole()
    {
        ColorfulConsole = true;
        return this;
    }
}
