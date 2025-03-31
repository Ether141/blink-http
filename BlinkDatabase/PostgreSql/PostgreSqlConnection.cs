using BlinkDatabase.General;
using Npgsql;
using System.Data.Common;

namespace BlinkDatabase.PostgreSql;

/// <summary>
/// Represents a connection to a PostgreSQL database.
/// </summary>
public class PostgreSqlConnection : IDatabaseConnection
{
    private readonly string? host;
    private readonly string? username;
    private readonly string? password;
    private readonly string? database;

    /// <summary>
    /// Gets a value indicating whether the connection to the database is established.
    /// </summary>
    public bool IsConnected { get; private set; }

    /// <summary>
    /// Gets the connection string used to connect to the database.
    /// </summary>
    public string? ConnectionString { get; }

    /// <summary>
    /// Gets the database connection object.
    /// </summary>
    public DbConnection? Connection { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgreSqlConnection"/> class with the specified connection parameters.
    /// </summary>
    /// <param name="host">The host of the PostgreSQL server.</param>
    /// <param name="username">The username to connect to the PostgreSQL server.</param>
    /// <param name="password">The password to connect to the PostgreSQL server.</param>
    /// <param name="database">The name of the database to connect to.</param>
    public PostgreSqlConnection(string? host, string? username, string? password, string? database)
    {
        this.host = host;
        this.username = username;
        this.password = password;
        this.database = database;

        ConnectionString = BuildConnectionString();
    }

    /// <summary>
    /// Establishes a connection to the PostgreSQL database.
    /// </summary>
    /// <returns>The established database connection.</returns>
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

    /// <summary>
    /// Disposes the connection by closing it if it is open.
    /// </summary>
    public void Dispose()
    {
        if (IsConnected && Connection != null)
        {
            Connection.Close();
            Connection = null;
        }
    }
}
