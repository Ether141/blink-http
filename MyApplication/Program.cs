using BlinkDatabase;
using BlinkDatabase.Mapping;
using BlinkDatabase.PostgreSql;
using BlinkDatabase.Sql;
using BlinkHttp.Application;
using BlinkHttp.Configuration;
using Npgsql;

namespace MyApplication;

internal class Program
{
    public static async Task Main(string[] args)
    {
        PostgreSqlConnection connection = new PostgreSqlConnection("172.18.133.85", "postgres", "password", "postgres");
        NpgsqlConnection conn = connection.Connect();

        PostgreSqlRepository<Book> booksRepo = new PostgreSqlRepository<Book>(conn);
        IEnumerable<Book> books = booksRepo.Select(b => b.Id <= 10);

        foreach (var book in books)
        {
            Console.WriteLine(book);
        }


        //PostgreSqlRepository<Library> libraries = new PostgreSqlRepository<Library>(conn);
        //IEnumerable<Library> libs = libraries.Select(l => l.Type == LibraryType.Free && l.Id == 1);

        //foreach (var library in libs)
        //{
        //    Console.WriteLine(library);
        //}


        //PostgreSqlRepository<Address> addresses = new PostgreSqlRepository<Address>(conn);

        //foreach (var o in addresses.GetWhere(a => a.Id <= 10 && a.City!.Id == 4))
        //{
        //    Console.WriteLine(o);
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
