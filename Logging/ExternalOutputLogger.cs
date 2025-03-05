using System.Diagnostics;
using System.IO.Pipes;

namespace BlinkHttp.Logging;

internal class ExternalOutputLogger : IGeneralLogger
{
    private readonly string? format;
    private NamedPipeClientStream? pipe;
    private StreamWriter? writer;
    private Process? childProcess;

    private const string LoggerConsolePath = ".\\LoggerConsole\\LoggerConsole.exe";

    public ExternalOutputLogger(string? format)
    {
        this.format = format;

        AppDomain.CurrentDomain.ProcessExit += (_, _) => MainProcessExit();

        CreateExternalOutput();
    }

    public void Log(string message, string? loggerName, LogLevel logLevel)
    {
        if (writer != null)
        {
            message = LoggingHelper.GetFormattedString(message, loggerName, logLevel, format);
            writer.WriteLine(message);
        }
    }

    private void CreateExternalOutput()
    {
        childProcess = StartProcess();

        pipe = new NamedPipeClientStream(".", "pipe", PipeDirection.Out);
        pipe.Connect();

        writer = new StreamWriter(pipe) { AutoFlush = true };
        writer.WriteLine($"External console{Environment.NewLine}");
    }

    private static Process StartProcess()
    {
        if (!File.Exists(LoggerConsolePath))
        {
            throw new FileNotFoundException($"External logger is unvailable: executable file cannot be found: {LoggerConsolePath}");
        }

        Process process = new Process();
        process.StartInfo.FileName = LoggerConsolePath;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.CreateNoWindow = false;
        process.Start();

        return process;
    }

    private void MainProcessExit()
    {
        writer?.Dispose();
        pipe?.Dispose();
        childProcess?.Kill();

        writer = null;
        pipe = null;
        childProcess = null;
    }
}
