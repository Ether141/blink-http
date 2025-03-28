using BlinkDatabase.General;
using BlinkDatabase.PostgreSql;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.DependencyInjection;
using BlinkHttp.Handling;
using BlinkHttp.Http;
using Logging;

namespace BlinkHttp.Application;

/// <summary>
/// Main logic for a web application. Handles HTTP server, routing, authorization and database connection.
/// </summary>
public class WebApplication
{
    private bool isServerRunning;
    private HttpServer? server;

    public string StartMessage { get; internal set; } = "HTTP server started. Ctrl + C to stop.";
    public IConfiguration? Configuration { get; internal init; }
    public string[]? Prefixes { get; internal set; }
    public IAuthorizer? Authorizer { get; internal init; }
    public string? RoutePrefix { get; internal init; }
    internal ServicesContainer? DependencyInjector { get; init; }

    private readonly ILogger logger = Logger.GetLogger<WebApplication>();

    /// <summary>
    /// Starts a web application and an HTTP server as an asynchronous operation and blocks the main thread, until the server stops.
    /// </summary>
    public async Task Run(string[] args) => await StartServer();

    private async Task StartServer()
    {
        if (Prefixes == null && Configuration != null)
        {
            Prefixes = Configuration.GetArray("server:prefixes") ?? throw new ArgumentNullException("server:prefix options cannot be found in the configuration file.");
        }
        else
        {
            throw new NullReferenceException("Configuration is not provided.");
        }

        Console.CancelKeyPress += ConsoleExit;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => isServerRunning = false;

        ControllersFactory.Initialize(DependencyInjector!);
        server = new HttpServer(Authorizer, Configuration, RoutePrefix, Prefixes);

        Task serverTask = server.StartAsync();

        isServerRunning = true;

        logger.Info(StartMessage);

        while (isServerRunning) { }

        server.Stop();

        await serverTask;
    }

    private void ConsoleExit(object? sender, ConsoleCancelEventArgs e)
    {
        isServerRunning = false;
        e.Cancel = true;
    }
}