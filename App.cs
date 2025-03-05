using BlinkHttp.Configuration;
using BlinkHttp.Http;

namespace BlinkHttp
{
    internal class App
    {
        private bool isServerRunning;

        private const string StartMessage = "HTTP server started. Ctrl + C to stop.";
        internal ApplicationConfiguration Configuration { get; }

        public App()
        {
            Configuration = new ApplicationConfiguration();
            Configuration.LoadConfiguration();
        }

        internal async Task Run(string[] args)
        {
            await StartServer();
        }

        private async Task StartServer()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            string[]? prefixes = Configuration.GetArray("server:prefixes") ?? throw new ArgumentNullException("server:prefix options cannot be found in the configuration file.");
            HttpServer server = new HttpServer(prefixes);
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
