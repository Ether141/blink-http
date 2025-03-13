using BlinkHttp.Authentication.Session;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using Logging;
using BlinkDatabase.General;

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
    private IDatabaseConnection? databaseConnection;
    private string? routePrefix;

    /// <summary>
    /// Associate <seealso cref="IConfiguration"/> which will be used by <seealso cref="WebApplication"/> later.
    /// </summary>
    public WebApplicationBuilder UseConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
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
    /// Enables authorization based on session.
    /// </summary>
    public WebApplicationBuilder UseSessionAuthorization()
    {
        authorizer = GetSessionManager(null);
        return this;
    }

    /// <summary>
    /// Enables authorization based on session and allows to configure such authorization.
    /// </summary>
    public WebApplicationBuilder UseSessionAuthorization(Action<SessionOptions> opt)
    {
        SessionOptions sessionOptions = new SessionOptions();
        opt.Invoke(sessionOptions);
        authorizer = GetSessionManager(sessionOptions);
        return this;
    }

    /// <summary>
    /// Adds support for <seealso cref="IDatabaseConnection"/>. Then this connection can be used for creating <seealso cref="IRepository{T}"/>.
    /// </summary>
    public WebApplicationBuilder UseDatabase(IDatabaseConnection connection)
    {
        databaseConnection = connection;
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
            DatabaseConnection = databaseConnection,
            RoutePrefix = routePrefix,
            Prefixes = prefixes
        };

        if (startMessage != null)
        {
            app.StartMessage = startMessage;
        }

        return app;
    }

    private static SessionManager GetSessionManager(SessionOptions? opt)
    {
        DatabaseUserInfoProvider userInfoProvider = new DatabaseUserInfoProvider();
        SessionStorageInMemory sessionStorage = new SessionStorageInMemory();
        AuthenticationProvider authenticationProvider = new AuthenticationProvider(userInfoProvider);
        SessionManager sessionManager = new SessionManager(sessionStorage, authenticationProvider, userInfoProvider);

        if (opt != null)
        {
            if (opt.AttemptsLimitingEnabled)
            {
                sessionManager.EnableAttemptsLimiting(opt.AttemptsLimitingCooldown, opt.AttemptsLimitPerCooldown); 
            }

            if (opt.SessionValidFor != null)
            {
                sessionManager.EnableSessionExpiration(opt.SessionValidFor.Value);
            }
        }

        return sessionManager;
    }
}
