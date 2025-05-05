using BlinkHttp.Authentication;
using BlinkHttp.Background;
using BlinkHttp.Configuration;
using BlinkHttp.DependencyInjection;
using BlinkHttp.Handling;
using BlinkHttp.Http;
using BlinkHttp.Routing;
using BlinkHttp.Server;
using BlinkHttp.Swagger;
using BlinkHttp.Logging;

namespace BlinkHttp.Application;

/// <summary>
/// Main logic for a web application. Handles HTTP server, routing, authorization, background services and database connection.
/// </summary>
public class WebApplication
{
    private bool isServerRunning;
    private IServer server;
    private BackgroundServicesManager? backgroundServicesManager;

    /// <summary>
    /// Gets the configuration instance used by the application. Not null only if conifguration was turned on during building of <see cref="WebApplication"/>.
    /// </summary>
    public IConfiguration? Configuration { get; }

    /// <summary>
    /// Gets the authorizer instance used for handling authorization. Not null only if authorization was turned on during building of <see cref="WebApplication"/>.
    /// </summary>
    public IAuthorizer? Authorizer { get; }

    /// <summary>
    /// Determines whether the application is running in a development environment (i.e. DEBUG is defined).
    /// </summary>
    /// <returns>
    /// <c>true</c> if the application is running in a development environment; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDevelopment
    {
        get
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }

    private readonly ServicesContainer services;
    private readonly RequestsHandler handler;

    private readonly IAuthorizer? authorizer;

    private readonly ILogger logger = LoggerFactory.Create<WebApplication>();

    internal WebApplication(IServer server,
                            ServicesContainer services,
                            IAuthorizer? authorizer,
                            IConfiguration? configuration,
                            IMiddleware[] middlewares,
                            string? routePrefix,
                            CorsOptions? corsOptions,
                            bool useSwagger,
                            BackgroundServicesManager? backgroundServicesManager)
    {
        this.server = server;
        this.services = services;
        this.authorizer = authorizer;
        this.backgroundServicesManager = backgroundServicesManager;

        Router router = ConfigureRouter(routePrefix, useSwagger);
        handler = new RequestsHandler(router, authorizer, middlewares, routePrefix, corsOptions);
    }

    /// <summary>
    /// Starts a web application and an HTTP server as an asynchronous operation and blocks the main thread, until the server stops.
    /// </summary>
    public async Task Run(string[] args) => await StartServer();

    private async Task StartServer()
    {
        Console.CancelKeyPress += ConsoleExit;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => isServerRunning = false;

        backgroundServicesManager?.StartAllServices();
        ControllersFactory.Initialize(services);

        server.RequestReceived += handler.HandleRequestAsync;
        Task serverTask = server.StartAsync();

        isServerRunning = true;

        while (isServerRunning) { }

        server.Stop();

        await StopAllBackgroundServicesAsync();
        await serverTask;
    }

    private Router ConfigureRouter(string? routePrefix, bool useSwagger)
    {
        Router router = new Router();
        router.Options.RoutePrefix = routePrefix;

        if (!useSwagger)
        {
            router.Options.IgnoredControllerTypes = [typeof(SwaggerController)];
        }

        router.InitializeAllRoutes();

        if (useSwagger)
        {
            ((SwaggerUI)services.Installator.SingletonInstances[typeof(SwaggerUI)]).GenerateJson(router.Routes);

            if (!IsDevelopment)
            {
                logger.Warning("You are using Swagger on the production environment.");
            }
        }

        return router;
    }

    private async Task StopAllBackgroundServicesAsync()
    {
        if (backgroundServicesManager != null)
        {
            await backgroundServicesManager.StopAllServicesAsync();
        }

        await Task.CompletedTask;
    }

    private void ConsoleExit(object? sender, ConsoleCancelEventArgs e)
    {
        isServerRunning = false;
        e.Cancel = true;
    }
}