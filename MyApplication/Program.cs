using BlinkHttp.Application;
using BlinkHttp.Configuration;

namespace MyApplication;

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = new WebApplicationBuilder();
        ApplicationConfiguration config = GetConfiguration();

        builder.Services
            .AddConfiguration(config)
            .AddBackgroundService(new EmailBackgroundService(), false);
            //.AddPostgreSql()
            //.AddSingleton<IUserInfoProvider, UserInfoProvider>();

        builder
            .ConfigureLogging(ConfigureLogging)
            .UseConfiguration()
            .AddGlobalCORS()
            .SetRoutePrefix(config["route_prefix"]);
            //.UseSessionAuthorization(opt => opt.EnableSessionExpiration(TimeSpan.FromHours(12)));

        WebApplication app = builder.Build();
        await app.Run(args);
    }

    private static void ConfigureLogging(BlinkHttp.Logging.LoggerSettings settings) 
        => settings.UseConsole()
                   .UseFile()
                   .SetConsoleLogFormat("%T %level:\n\t%name => %scope %message")
                   .EnableColorfulConsole()
                   .SetFileLogFormat("%D %level [%name] %scope %message")
                   .EnableFileLogFooter("===");

    private static ApplicationConfiguration GetConfiguration()
    {
        ApplicationConfiguration config = new ApplicationConfiguration();
        config.LoadConfiguration();
        return config;
    }
}