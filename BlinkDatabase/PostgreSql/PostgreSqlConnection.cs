using BlinkDatabase.General;
using Npgsql;
using System.Data.Common;

namespace BlinkDatabase.PostgreSql;

public class PostgreSqlConnection : IDatabaseConnection
{
    private readonly string? host;
    private readonly string? username;
    private readonly string? password;
    private readonly string? database;

    public bool IsConnected { get; private set; }
    public string? ConnectionString { get; }
    public DbConnection? Connection { get; private set; }

    public PostgreSqlConnection(string? host, string? username, string? password, string? database)
    {
        this.host = host;
        this.username = username;
        this.password = password;
        this.database = database;

        ConnectionString = BuildConnectionString();
    }

    public DbConnection Connect()
    {
        Connection = new NpgsqlConnection(ConnectionString);
        Connection.Open();

        IsConnected = true;
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

    public void Dispose()
    {
        if (IsConnected && Connection != null)
        {
            Connection.Close();            
        }
    }
}
