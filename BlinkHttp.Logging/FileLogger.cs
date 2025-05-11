using System.Text;

namespace BlinkHttp.Logging;

internal class FileLogger : ILogSink, IDisposable
{
    internal string FilePath { get; }

    public string? Format { get; set; }
    public string? FileLogFooter { get; set; }

    private int writeOperations = 0;
    private FileStream? stream;
    private bool disposedValue;

    public FileLogger(string? filePath)
    {
        FilePath = filePath ?? "./logs.txt";
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

    public void Log(LogMessage message) => WriteText(message.GetFormattedMessage(Format));

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

            if (Interlocked.Increment(ref writeOperations) == 10)
            {
                Interlocked.Exchange(ref writeOperations, 0);
                stream.Flush();
            }
        }
        catch { }
    }

    private void CreateNewLogFile()
    {
        if (stream != null)
        {
            stream.Flush();
            stream.Close();
            string ext = Path.GetExtension(FilePath);
            File.Move(FilePath, FilePath[..ext.Length] + DateTime.Now.ToString() + ext);
        }

        stream = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (FileLogFooter != null)
            {
                WriteText(FileLogFooter);
            }
            
            stream?.Flush();
            stream?.Close();
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