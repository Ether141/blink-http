using BlinkHttp.Authentication;
using BlinkHttp.Authentication.Session;
using BlinkHttp.Background;
using BlinkHttp.Configuration;
using BlinkHttp.DependencyInjection;
using BlinkHttp.Http;
using BlinkHttp.Logging;
using BlinkHttp.Server;
using BlinkHttp.Server.Default;
using BlinkHttp.Swagger;
using BlinkHttp.Logging;

namespace BlinkHttp.Application;

/// <summary>
/// Allows to build <seealso cref="WebApplication"/> and configure its features.
/// </summary>
public class WebApplicationBuilder
{
    private IConfiguration? configuration;
    private string[]? prefixes;
    private IAuthorizer? authorizer;
    private string? routePrefix;
    private Func<IServer>? serverProvider;
    private CorsOptions? corsOptions;
    private bool useSwagger = false;
    private BackgroundServicesManager? backgroundServicesManager;

    /// <summary>
    /// Allows to define services for dependency injection, which will be used across application.
    /// </summary>
    public ServicesContainer Services { get; } = new ServicesContainer();

    /// <summary>
    /// Tells <seealso cref="WebApplication"/> that is should use <seealso cref="IConfiguration"/> from Services container, which will be used later.
    /// </summary>
    public WebApplicationBuilder UseConfiguration()
    {
        configuration = Services.Installator.GetSingletonByService<IConfiguration>();
        return this;
    }

    /// <summary>
    /// Adds HTTP prefix/es, on which server will be listening. There must be at least one prefix, if configuration is not provided.
    /// </summary>
    public WebApplicationBuilder AddPrefix(params string[] prefixes)
    {
        this.prefixes = prefixes;
        return this;
    }

    /// <summary>
    /// Enables authorization based on session and supplies it with <seealso cref="IUserInfoProvider"/> which is required to obtain information about user priviliges and password.
    /// </summary>
    public WebApplicationBuilder UseSessionAuthorization()
    {
        authorizer = GetSessionManager(Services.Installator.GetSingletonByService<IUserInfoProvider>(), null);

        if (authorizer != null)
        {
            Services.AddSingleton<IAuthorizer, SessionManager>((SessionManager)authorizer);
        }

        return this;
    }

    /// <summary>
    /// Enables authorization based on session and supplies it with <seealso cref="IUserInfoProvider"/> which is required to obtain information about user priviliges and password. Allows to configure such authorization.
    /// </summary>
    public WebApplicationBuilder UseSessionAuthorization(Action<SessionOptions> opt)
    {
        SessionOptions sessionOptions = new SessionOptions();
        opt.Invoke(sessionOptions);
        authorizer = GetSessionManager(Services.Installator.GetSingletonByService<IUserInfoProvider>(), sessionOptions);
        Services.AddSingleton<IAuthorizer, SessionManager>((SessionManager)authorizer);
        return this;
    }

    /// <summary>
    /// Configures logging based on the provided logger settings.
    /// </summary>
    public WebApplicationBuilder ConfigureLogging(Action<LoggerSettings> settings)
    {
        LoggerSettings sett = new LoggerSettings();
        settings.Invoke(sett);
        LoggerFactory.CreateFactory(sett);
        return this;
    }

    /// <summary>
    /// Sets optional route prefix, for all routes, e.g. "api".
    /// </summary>
    public WebApplicationBuilder SetRoutePrefix(string? prefix)
    {
        routePrefix = prefix;
        return this;
    }

    /// <summary>
    /// Adds Cross-Origin Resource Sharing (CORS) support to the application for all endpoints. By default, all headers, methods and origins are accepted and credentials are disabled.
    /// </summary>
    public WebApplicationBuilder AddGlobalCORS() => AddGlobalCORS(opt => { });

    /// <summary>
    /// Adds Cross-Origin Resource Sharing (CORS) support to the application for all endpoints, with the specified CORS options.
    /// </summary>
    public WebApplicationBuilder AddGlobalCORS(Action<CorsOptions> opt)
    {
        corsOptions = new CorsOptions();
        opt.Invoke(corsOptions);
        return this;
    }

    /// <summary>
    /// Configures Swagger documentation for the application with the specified title and version.
    /// </summary>
    public WebApplicationBuilder ConfigureSwagger(string title, string version) => ConfigureSwagger(title, version, []);

    /// <summary>
    /// Configures Swagger documentation for the application with the specified title and version, and optional metadata for selected endpoints.
    /// </summary>
    public WebApplicationBuilder ConfigureSwagger(string title, string version, params EndpointMetadata[] metadata)
    {
        SwaggerUI ui = new SwaggerUI("api/docs/swagger.json", title, version, metadata);
        Services.AddSingleton<SwaggerUI>(ui);
        useSwagger = true;
        return this;
    }

    /// <summary>
    /// Builds new instance of <seealso cref="WebApplication"/> and configure its features.
    /// </summary>
    public WebApplication Build() =>
        new WebApplication((serverProvider ?? GetDefaultServer).Invoke(),
                           Services,
                           authorizer,
                           configuration,
                           Services.Installator.ResolveMiddlewares(),
                           routePrefix,
                           corsOptions,
                           useSwagger,
                           GetBackgroundServicesManager());

    private BackgroundServicesManager? GetBackgroundServicesManager()
    {
        if (Services.Installator.BackgroundServices.Count > 0)
        {
            backgroundServicesManager = new BackgroundServicesManager(Services.Installator.BackgroundServices);
            Services.AddSingleton<IBackgroundServices, BackgroundServicesManager>(backgroundServicesManager);
        }

        return backgroundServicesManager;
    }

    private IServer GetDefaultServer()
    {
        if (prefixes == null && configuration != null)
        {
            prefixes = configuration.GetArray("server:prefixes") ?? throw new ArgumentNullException("server:prefix options cannot be found in the configuration file.");
        }
        else
        {
            throw new NullReferenceException("Configuration is not provided.");
        }

        return new SimpleServer(prefixes);
    }

    private static SessionManager GetSessionManager(IUserInfoProvider userInfoProvider, SessionOptions? opt)
    {
        SessionStorageInMemory sessionStorage = new SessionStorageInMemory();
        AuthenticationProvider authenticationProvider = new AuthenticationProvider(userInfoProvider);
        SessionManager sessionManager = new SessionManager(sessionStorage, authenticationProvider, userInfoProvider);

        if (opt != null)
        {
            if (opt.SessionValidFor != null)
            {
                sessionManager.EnableSessionExpiration(opt.SessionValidFor.Value);
            }
        }

        return sessionManager;
    }
}
