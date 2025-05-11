namespace BlinkHttp.Logging;

internal class ConsoleLogger : ILogSink
{
    public string? Format { get; set; }
    internal bool Colorful { get; set; }

    private static readonly object _lock = new object();

    private static readonly Dictionary<string, ConsoleColor> colors = new() 
    { 
        { "INFO", ConsoleColor.Blue }, 
        { "DEBUG", ConsoleColor.DarkGray },
        { "WARN", ConsoleColor.DarkYellow },
        { "ERROR", ConsoleColor.Red },
        { "CRIT", ConsoleColor.DarkRed },
    };

    public void Log(LogMessage logMessage) => Write(logMessage.GetFormattedMessage(Format));

    private void Write(string msg)
    {
        if (!Colorful)
        {
            Console.WriteLine(msg);
            return;
        }

        int index = -1;
        string? t = null;

        foreach (string text in colors.Keys)
        {
            index = msg.IndexOf(text);

            if (index > -1)
            {
                t = text;
                break;
            }
        }

        if (t == null)
        {
            Console.WriteLine(msg);
            return;
        }

        lock (_lock)
        {
            Console.Write(msg[..index]);
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = colors[t];
            Console.Write(msg[index..(index + t.Length)]);
            Console.ForegroundColor = color;
            Console.WriteLine(msg[(index + t.Length)..]);
        }
    }
}
