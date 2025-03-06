using BlinkHttp.Application;
using BlinkHttp.Configuration;

namespace MyApplication;

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = new WebApplicationBuilder();

        builder
            .UseConfiguration(GetConfiguration())
            .ConfigureLogging(settings => settings.UseConsole(opt => opt.UseStandardOutput()));

        WebApplication app = builder.Build();
        await app.Run(args);
    }

    private static IConfiguration GetConfiguration()
    {
        ApplicationConfiguration config = new ApplicationConfiguration();
        config.LoadConfiguration();
        return config;
    }
}
