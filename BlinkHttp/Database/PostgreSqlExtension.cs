using BlinkDatabase.PostgreSql;
using BlinkHttp.Application;

namespace BlinkHttp.Database;

public static class PostgreSqlExtension
{
    /// <summary>
    /// Enables support for PostgreSQL database provider and configure connection info, which will be used in creation of database connections.
    /// </summary>
    public static WebApplicationBuilder UsePostgreSql(this WebApplicationBuilder builder, string host, string username, string password, string database)
    {
        builder.UseDatabase(new PostgreSqlConnection(host, username, password, database));
        return builder;
    }
}
