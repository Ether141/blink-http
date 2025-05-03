using BlinkHttp.Configuration;
using BlinkHttp.DependencyInjection;

namespace BlinkDatabase.PostgreSql;

public static class PostgreSqlServiceExtension
{
    /// <summary>
    /// Adds new <seealso cref="PostgreSqlConnection"/> as singleton, which will be used for handling database operations and supplying new <seealso cref="IRepository{T}"/>.
    /// </summary>
    /// <remarks>Note: When you add a database connection using this method, firstly you also need to add <seealso cref="IConfiguration"/> using the AddConfiguration method.</remarks>
    public static ServicesContainer AddPostgreSql(this ServicesContainer container)
    {
        IConfiguration configuration = container.Configuration ?? throw new InvalidOperationException("Unable to add PostgreSql support, because configuration is not set up yet");
        bool loggingOn = configuration["sql:logging_on"] != null ? configuration.Get<bool>("sql:logging_on") : false;
        AddPostgreSql(container, configuration.Get("sql:hostname")!, configuration.Get("sql:username")!, configuration.Get("sql:password")!, configuration.Get("sql:database")!, loggingOn);
        return container;
    }

    /// <summary>
    /// Adds new <seealso cref="PostgreSqlConnection"/> as singleton, which will be used for handling database operations and supplying new <seealso cref="IRepository{T}"/>.
    /// </summary>
    public static ServicesContainer AddPostgreSql(this ServicesContainer container, string hostname, string username, string password, string database, bool loggingOn)
    {
        PostgreSqlConnection conn = new PostgreSqlConnection(hostname, username, password, database);
        conn.SqlQueriesLogging = loggingOn;
        container.AddRepository(conn, typeof(PostgreSqlRepository<>));
        return container;
    }
}