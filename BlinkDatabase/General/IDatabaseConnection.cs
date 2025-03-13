﻿using System.Data.Common;

namespace BlinkDatabase.General;

/// <summary>
/// Exposes properties and methods to handle connection with the database.
/// </summary>
public interface IDatabaseConnection : IDisposable
{
    bool IsConnected { get; }
    string? ConnectionString { get; }
    DbConnection? Connection { get; }
    DbConnection Connect();
}
