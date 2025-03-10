using BlinkDatabase;
using BlinkDatabase.General;
using BlinkDatabase.Mapping;
using BlinkDatabase.PostgreSql;
using BlinkDatabase.Utilities;
using BlinkHttp.Application;
using BlinkHttp.Configuration;
using Npgsql;

namespace MyApplication;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Logging.Logger.Configure(settings => settings.UseConsole(opt => opt.UseStandardOutput()));

        Book book = new Book() { Id = 1, Library = new Library() { Id = 1 }};

        PostgreSqlConnection connection = new PostgreSqlConnection("172.18.133.85", "postgres", "password", "postgres");
        NpgsqlConnection conn = connection.Connect();

        PostgreSqlRepository<User> usersRepo = new PostgreSqlRepository<User>(conn);

        //User user = usersRepo.SelectSingle(u => u.Id == 10)!;
        //user.Role!.Id = 1;
        //usersRepo.Update(user);

        IEnumerable<User> allUsers = usersRepo.Select(u => u.Role.Id == 2).OrderBy(u => u.Id);

        foreach (User u in allUsers)
        {
            Console.WriteLine(DatabaseObjectToString.ToString(u, false));
        }


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



        //WebApplicationBuilder builder = new WebApplicationBuilder();

        //builder
        //    .UseConfiguration(GetConfiguration())
        //    .ConfigureLogging(settings => settings.UseConsole(opt => opt.UseStandardOutput()));

        //WebApplication app = builder.Build();
        //await app.Run(args);
    }

    private static IConfiguration GetConfiguration()
    {
        ApplicationConfiguration config = new ApplicationConfiguration();
        config.LoadConfiguration();
        return config;
    }
}
