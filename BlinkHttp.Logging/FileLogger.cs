using System.Text;

namespace BlinkHttp.Logging;

internal class FileLogger : ILogSink, IDisposable
{
    internal string FilePath { get; }

    private readonly string? format;

    private int writeOperations = 0;
    private FileStream? stream;
    private bool disposedValue;

    public FileLogger(string? filePath, string? format)
    {
        FilePath = filePath ?? "./logs.txt";
        this.format = format;
        OpenFileStream();
    }

    private void OpenFileStream()
    {
        if (stream != null)
        {
            return;
        }

        stream = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
    }

    public void Log(LogMessage message) => WriteText(message.GetFormattedMessage(format));

    private void WriteText(string text)
    {
        if (stream == null)
        {
            return;
        }

        try
        {
            text += Environment.NewLine;

            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(text));

            stream.Write(bytes);

            if (Interlocked.Increment(ref writeOperations) == 20)
            {
                Interlocked.Exchange(ref writeOperations, 0);
                stream.Flush();
            }
        }
        catch { }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            stream?.Flush();
            stream?.Dispose();
            stream = null;
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}