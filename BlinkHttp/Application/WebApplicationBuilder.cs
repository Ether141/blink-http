using BlinkHttp.Authentication.Session;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using Logging;
using BlinkDatabase.General;
using BlinkHttp.DependencyInjection;
using BlinkDatabase.PostgreSql;

namespace BlinkHttp.Application;

/// <summary>
/// Allows to build <seealso cref="WebApplication"/> and configure its features.
/// </summary>
public class WebApplicationBuilder
{
    private IConfiguration? configuration;
    private string[]? prefixes;
    private string? startMessage;
    private IAuthorizer? authorizer;
    private string? routePrefix;

    /// <summary>
    /// Allows to define services for dependency injection, which will be used across application.
    /// </summary>
    public ServicesContainer Services { get; } = new ServicesContainer();

    /// <summary>
    /// Tells <seealso cref="WebApplication"/> that is should use <seealso cref="IConfiguration"/> from Serivces container, which will be used later.
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
    /// Sets start message, which will be displayed in logs, when server is started.
    /// </summary>
    public WebApplicationBuilder SetStartMessage(string startMessage)
    {
        this.startMessage = startMessage;
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
    /// Sets optional route prefix, for all routes, e.g. "api".
    /// </summary>
    public WebApplicationBuilder SetRoutePrefix(string? prefix)
    {
        routePrefix = prefix;
        return this;
    }

    /// <summary>
    /// Builds new instance of <seealso cref="WebApplication"/> and configure its features.
    /// </summary>
    public WebApplication Build()
    {
        WebApplication app = new WebApplication
        {
            Configuration = configuration,
            Authorizer = authorizer,
            RoutePrefix = routePrefix,
            Prefixes = prefixes,
            DependencyInjector = Services
        };

        if (startMessage != null)
        {
            app.StartMessage = startMessage;
        }

        return app;
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
