using BlinkHttp.Authentication.Session;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using Logging;

namespace BlinkHttp.Application;

public class WebApplicationBuilder
{
    private IConfiguration? configuration;
    private string[]? prefixes;
    private string? startMessage;
    private IAuthorizer? authorizer;

    public WebApplicationBuilder UseConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
        return this;
    }

    public WebApplicationBuilder ConfigureLogging(Action<LoggerSettings> settings)
    {
        LoggerSettings loggerSettings = new LoggerSettings();
        Logger.Configure(settings);
        return this;
    }

    public WebApplicationBuilder AddPrefix(params string[] prefixes)
    {
        this.prefixes = prefixes;
        return this;
    }

    public WebApplicationBuilder SetStartMessage(string startMessage)
    {
        this.startMessage = startMessage;
        return this;
    }

    public WebApplicationBuilder UseSessionAuthorization()
    {
        authorizer = GetSessionManager(null);
        return this;
    }

    public WebApplicationBuilder UseSessionAuthorization(Action<SessionOptions> opt)
    {
        SessionOptions sessionOptions = new SessionOptions();
        opt.Invoke(sessionOptions);
        authorizer = GetSessionManager(sessionOptions);
        return this;
    }

    public WebApplication Build()
    {
        WebApplication app = new WebApplication
        {
            Configuration = configuration,
            Authorizer = authorizer
        };

        if (startMessage != null)
        {
            app.StartMessage = startMessage;
        }

        if (prefixes != null)
        {
            app.Prefixes = prefixes;
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
