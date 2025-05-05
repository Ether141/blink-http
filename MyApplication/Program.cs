using BlinkDatabase.PostgreSql;
using BlinkHttp.Application;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.Http;
using BlinkHttp.Logging;
using BlinkHttp.Swagger;
using System.Net;

namespace MyApplication;

internal class Program
{
    public static async Task Main(string[] args)
    {
        //Book book = new Book() { Id = 1, Library = new Library() { Id = 1 }};

        //PostgreSqlConnection conn = new PostgreSqlConnection("172.18.133.85", "postgres", "password", "postgres");

        //PostgreSqlRepository<User> usersRepo = new PostgreSqlRepository<User>(conn);

        //User user = usersRepo.SelectSingle(u => u.Id == 10)!;
        //user.Role!.Id = 1;
        //usersRepo.Update(user);

        //IEnumerable<User> allUsers = usersRepo.Select(u => u.Nickname == "mariad_art");

        //foreach (User u in allUsers)
        //{
        //    Console.WriteLine(DatabaseObjectToString.ToString(u, false));
        //}


        //PostgreSqlRepository<Book> booksRepo = new PostgreSqlRepository<Book>(conn);

        //IEnumerable<Book> books = booksRepo.Select();

        //foreach (var b in books)
        //{
        //    Console.WriteLine(b);
        //}


        //PostgreSqlRepository<Library> libraries = new PostgreSqlRepository<Library>(conn);
        //IEnumerable<Library> libs = libraries.Select(l => l.Type == LibraryType.Free && l.Id == 1);

        //foreach (var library in libs)
        //{
        //    Console.WriteLine(library);
        //}

        WebApplicationBuilder builder = new WebApplicationBuilder();

        builder
            .ConfigureLogging(s => s.UseConsole());

        ApplicationConfiguration config = GetConfiguration(); // TODO: change way of handling configuration - move it to WebApplicationBuilder

        builder.Services
            .AddConfiguration(config)
            .AddBackgroundService(new EmailBackgroundService(), false);
            //.AddPostgreSql()
            //.AddSingleton<IUserInfoProvider, UserInfoProvider>();

        builder
            .UseConfiguration()
            .AddGlobalCORS(opt =>
            {
                opt.Origin = "main";
                opt.Headers = "main";
                opt.Methods = "main";
                opt.Credentials = true;
            })
            .SetRoutePrefix(config["route_prefix"]);
            //.UseSessionAuthorization(opt => opt.EnableSessionExpiration(TimeSpan.FromHours(12)));

        WebApplication app = builder.Build();
        await app.Run(args);
    }

    private static ApplicationConfiguration GetConfiguration()
    {
        ApplicationConfiguration config = new ApplicationConfiguration();
        config.LoadConfiguration();
        return config;
    }
}