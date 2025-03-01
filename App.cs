using BlinkHttp.Http;

namespace BlinkHttp
{
    internal class App
    {
        private bool isServerRunning;

        private const string StartMessage = "HTTP server started. Listening on port: 8080. Ctrl + C to stop.";
        private const int Port = 8080;

        private string Address = $"http://localhost:{Port}/";

        internal async Task Run(string[] args)
        {
            await StartServer();
        }

        private async Task StartServer()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            HttpServer server = new HttpServer(Address);
            Task serverTask = server.StartAsync();

            isServerRunning = true;

            Console.WriteLine(StartMessage);

            while (isServerRunning) { }

            server.Stop();
            await serverTask;
        }

        private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            isServerRunning = false;
            e.Cancel = true;
        }
    }
}
