using Npgsql;

namespace BlinkDatabase.PostgreSql;

public class PostgreSqlConnection
{
    private readonly string? host;
    private readonly string? username;
    private readonly string? password;
    private readonly string? database;

    public string? ConnectionString { get; }
    public NpgsqlConnection? Connection { get; private set; }

    public PostgreSqlConnection(string? host, string? username, string? password, string? database)
    {
        this.host = host;
        this.username = username;
        this.password = password;
        this.database = database;

        ConnectionString = BuildConnectionString();
    }

    public NpgsqlConnection Connect()
    {
        Connection = new NpgsqlConnection(ConnectionString);
        Connection.Open();
        return Connection;
    }

    private string BuildConnectionString()
    {
        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Username = username,
            Database = database,
            Password = password
        };

        return builder.ToString();
    }
}
