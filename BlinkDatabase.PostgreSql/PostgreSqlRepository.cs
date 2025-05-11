using BlinkDatabase.Annotations;
using BlinkDatabase.General;
using BlinkDatabase.Mapping;
using BlinkDatabase.Processing;
using Npgsql;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace BlinkDatabase.PostgreSql;

/// <summary>
/// Allows to easily retrieve, insert, update or delete entites from PostgreSQL table, using given entity type. Repository is reflection of single table in the database.
/// </summary>
/// <typeparam name="T">Type of entity class, which is marked properly with all requied attributes. Must be class and has parameterless constructor.</typeparam>
public class PostgreSqlRepository<T> : IRepository<T> where T : class, new()
{
    /// <summary>
    /// Name of table which is associatd with this repository.
    /// </summary>
    public string TableName { get; }

    private readonly SqlMapper<T> mapper;
    private readonly PostgreSqlConnection connection;

    /// <summary>
    /// Create new instance of a <seealso cref="PostgreSqlRepository{T}"/> with given connection, and opens this connection if it's not already.
    /// </summary>
    /// <param name="connection"></param>
    public PostgreSqlRepository(PostgreSqlConnection connection)
    {
        if (!connection.IsConnected)
        {
            connection.Connect();
        }

        this.connection = connection;
        Type type = typeof(T);

        TableName = type.GetCustomAttribute<TableAttribute>()!.TableName;
        mapper = new SqlMapper<T>((DbDataReader reader, int ordinal) => ((NpgsqlDataReader)reader).GetPostgresType(ordinal).Name);
    }

    public IEnumerable<T> Select()
    {
        string query = SqlQueriesBuilder.SelectAll<T>();
        return ExecuteSelect(query);
    }

    public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
    {
        string query = SqlQueriesBuilder.SelectWhere(expression);
        return ExecuteSelect(query);
    }

    public T? SelectSingle(Expression<Func<T, bool>> expression)
    {
        string query = SqlQueriesBuilder.SelectWhere(expression);
        return ExecuteSelect(query).FirstOrDefault();
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        string query = SqlQueriesBuilder.SelectExist(expression);
        return ExecuteSelectNotMap(query) == 1;
    }

    public bool Exists(int id)
    {
        string query = SqlQueriesBuilder.SelectExist<T>(id);
        return ExecuteSelectNotMap(query) == 1;
    }

    public void Insert(T obj)
    {
        string query = SqlQueriesBuilder.Insert(obj);
        object? id = ExecuteScalar(query);
        mapper.UpdateId(id, obj);
    }

    public int Update(T obj)
    {
        string query = SqlQueriesBuilder.Update(obj);
        return ExecuteNonQuery(query);
    }

    public int Delete(Expression<Func<T, bool>> expression)
    {
        string query = SqlQueriesBuilder.Delete(expression);
        return ExecuteNonQuery(query);
    }

    private int ExecuteNonQuery(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = GetCommand(query);
            return cmd.ExecuteNonQuery();
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private object? ExecuteScalar(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = GetCommand(query);
            return cmd.ExecuteScalar();
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private IEnumerable<T> ExecuteSelect(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = GetCommand(query);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            return mapper.MapAll(reader);
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private int ExecuteSelectNotMap(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = GetCommand(query);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            return mapper.Count(reader);
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private NpgsqlCommand GetCommand(string query)
    {
        connection.LogSqlQuery(query);
        return new NpgsqlCommand(query, (NpgsqlConnection)connection.Connection!);
    }
}
