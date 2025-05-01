using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.DependencyInjection;
using BlinkHttp.Handling;
using BlinkHttp.Server;
using Logging;

namespace BlinkHttp.Application;

/// <summary>
/// Main logic for a web application. Handles HTTP server, routing, authorization and database connection.
/// </summary>
public class WebApplication
{
    private bool isServerRunning;
    private IServer server;

    /// <summary>
    /// Gets the configuration instance used by the application. Not null only if conifguration was turned on during building of <see cref="WebApplication"/>.
    /// </summary>
    public IConfiguration? Configuration { get; }

    /// <summary>
    /// Gets the authorizer instance used for handling authorization. Not null only if authorization was turned on during building of <see cref="WebApplication"/>.
    /// </summary>
    public IAuthorizer? Authorizer { get; }

    private readonly ServicesContainer services;
    private readonly RequestsHandler handler;

    private readonly IAuthorizer? authorizer;
    private readonly string? routePrefix;

    private readonly ILogger logger = Logger.GetLogger<WebApplication>();

    internal WebApplication(IServer server, ServicesContainer services, IAuthorizer? authorizer, IConfiguration? configuration, IMiddleware[] middlewares, string? routePrefix)
    {
        this.server = server;
        this.services = services;

        this.authorizer = authorizer;
        this.routePrefix = routePrefix;

        handler = new RequestsHandler(authorizer, middlewares, routePrefix);
    }

    /// <summary>
    /// Starts a web application and an HTTP server as an asynchronous operation and blocks the main thread, until the server stops.
    /// </summary>
    public async Task Run(string[] args) => await StartServer();

    private async Task StartServer()
    {
        Console.CancelKeyPress += ConsoleExit;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => isServerRunning = false;

        ControllersFactory.Initialize(services);

        server.RequestReceived += handler.HandleRequestAsync;
        Task serverTask = server.StartAsync();

        isServerRunning = true;

        while (isServerRunning) { }

        server.Stop();
        Logger.CleanupLoggers();

        await serverTask;
    }

    private void ConsoleExit(object? sender, ConsoleCancelEventArgs e)
    {
        isServerRunning = false;
        e.Cancel = true;
    }
}