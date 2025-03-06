using BlinkHttp.Configuration;
using BlinkHttp.Logging;

namespace BlinkHttp.Application;

public class WebApplicationBuilder
{
    private IConfiguration? configuration;
    private string[]? prefixes;
    private string? startMessage;

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

    public WebApplication Build()
    {
        WebApplication app = new WebApplication();
        app.Configuration = configuration;

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
}
