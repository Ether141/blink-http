using System.Data.Common;

namespace BlinkDatabase.General;

/// <summary>
/// Exposes properties and methods to handle connection with the database.
/// </summary>
public interface IDatabaseConnection : IDisposable
{
    /// <summary>
    /// Indicates whether the connection with the database is established.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Connection string which is used to connect with the database.
    /// </summary>
    string? ConnectionString { get; }

    /// <summary>
    /// Indicates whether all SQL queries should be logged before execution.
    /// </summary>
    bool SqlQueriesLogging { get; set; }

    /// <summary>
    /// Currently held database connection.
    /// </summary>
    DbConnection? Connection { get; }

    /// <summary>
    /// Establishes connection with the database, and returns object which represents this connection.
    /// </summary>
    DbConnection Connect();
}
