namespace BlinkHttp.Logging;

public class FileLoggerOptions : LoggerOptions
{
    internal string? FilePath { get; private set; }

    public FileLoggerOptions SetFilePath(string? filePath)
    {
        FilePath = filePath;
        return this;
    }
}
