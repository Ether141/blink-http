using System.Text;

namespace BlinkHttp.Logging;

internal class FileLogger : IGeneralLogger
{
    internal string FilePath { get; }

    private readonly string? format;

    public FileLogger(string? filePath, string? format)
    {
        FilePath = filePath ?? "./logs.txt";
        this.format = format;
    }

    public void Log(string message, string? loggerName, LogLevel logLevel)
    {
        WriteText(LoggingHelper.GetFormattedString(message, loggerName, logLevel, format));
    }

    private void WriteText(string text)
    {
        try
        {
            text += Environment.NewLine;

            using FileStream stream = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(text));

            stream.Write(bytes);
        }
        catch { }
    }
}
