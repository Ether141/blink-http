namespace Logging;

internal class ConsoleLogger : IGeneralLogger
{
    private readonly string? format;
    private readonly bool colorful;
    private readonly ConsoleColor defaultColor = ConsoleColor.Gray;
    private readonly object _lock = new object();

    private Dictionary<string, ConsoleColor> keywords = new()
    {
        { "INFO", ConsoleColor.Blue },
        { "DEBUG", ConsoleColor.DarkGray },
        { "WARN", ConsoleColor.DarkYellow },
        { "ERROR", ConsoleColor.Red },
        { "CRIT", ConsoleColor.DarkRed}
    };

    public ConsoleLogger(string? format, bool colorful)
    {
        this.format = format;
        this.colorful = colorful;
    }

    public void Log(string message, string? loggerName, LogLevel logLevel) => Write(message, loggerName, logLevel);

    private void Write(string message, string? loggerName, LogLevel logLevel)
    {
        lock (_lock)
        {
            message = LoggingHelper.GetFormattedString(message, loggerName, logLevel, format);

            if (!colorful)
            {
                Console.WriteLine(message);
                return;
            }

            WriteColorfulMessage(message); 
        }
    }

    private void WriteColorfulMessage(string message)
    {
        Console.ForegroundColor = defaultColor;
        List<(int, int, ConsoleColor)> indexes = [];

        foreach ((string keyword, ConsoleColor color) in keywords)
        {
            string word = $" {keyword} ";
            int index = 0;

            while ((index = message.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                index++;
                indexes.Add((index, keyword.Length, color));
                index += keyword.Length;
            }
        }

        if (indexes.Count == 0)
        {
            Console.WriteLine(message);
            return;
        }

        List<(int ind, int len, ConsoleColor col)> ordered = [.. indexes.OrderBy(i => i.Item1)];

        ordered.Insert(0, (0, ordered[0].ind, defaultColor));
        Console.Write(message[..ordered[1].ind]);

        for (int i = 1; i < ordered.Count; i++)
        {
            Console.Write(message[(ordered[i - 1].ind + ordered[i - 1].len)..ordered[i].ind]);
            Console.ForegroundColor = ordered[i].col;
            Console.Write(message[ordered[i].ind..(ordered[i].ind + ordered[i].len)]);
            Console.ForegroundColor = defaultColor;
        }

        Console.ForegroundColor = defaultColor;
        Console.Write(message[(ordered[^1].ind + ordered[^1].len)..]);
        Console.WriteLine();
    }
}
