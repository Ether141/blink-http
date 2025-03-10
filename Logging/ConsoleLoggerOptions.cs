namespace Logging;

public class ConsoleLoggerOptions : LoggerOptions
{
    internal bool ColorfulConsole { get; private set; }
    internal bool StandardOutput { get; private set; }

    public ConsoleLoggerOptions EnableColorfulConsole()
    {
        ColorfulConsole = true;
        return this;
    }

    public ConsoleLoggerOptions UseStandardOutput()
    {
        StandardOutput = true;
        return this;
    }
}
