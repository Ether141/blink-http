using BlinkDatabase.Annotations;
using BlinkDatabase.General;
using BlinkDatabase.Mapping;
using Npgsql;
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

    internal List<ObjectFromDatabase> CurrentObjects { get; } = [];

    private readonly ObjectMapper<T> mapper;
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
        mapper = new ObjectMapper<T>(this);
    }

    public IEnumerable<T> Select()
    {
        string query = SqlSelectBuilder.SelectAll<T>();
        return ExecuteSelect(query);
    }

    public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
    {
        string query = SqlSelectBuilder.SelectWhere(expression);
        return ExecuteSelect(query);
    }

    public T? SelectSingle(Expression<Func<T, bool>> expression)
    {
        string query = SqlSelectBuilder.SelectWhere(expression);
        return ExecuteSelect(query).FirstOrDefault();
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        string query = SqlSelectBuilder.SelectExist(expression);
        ExecuteSelectNotMap(query);
        return CurrentObjects.Count == 1;
    }

    public bool Exists(int id)
    {
        string query = SqlSelectBuilder.SelectExist<T>(id);
        ExecuteSelectNotMap(query);
        return CurrentObjects.Count == 1;
    }

    public int Insert(T obj)
    {
        string query = SqlInsertBuilder.Insert(obj);
        return ExecuteNonQuery(query);
    }

    public int Update(T obj)
    {
        string query = SqlUpdateBuilder.Update(obj);
        return ExecuteNonQuery(query);
    }

    public int Delete(Expression<Func<T, bool>> expression)
    {
        string query = SqlDeleteBuilder.Delete(expression);
        return ExecuteNonQuery(query);
    }

    private int ExecuteNonQuery(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = new NpgsqlCommand(query, (NpgsqlConnection)connection.Connection!);
            return cmd.ExecuteNonQuery();
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
            using NpgsqlCommand cmd = new NpgsqlCommand(query, (NpgsqlConnection)connection.Connection!);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            List<T> result = [];
            ReadAllObjects(reader);

            while (CurrentObjects.Count > 0)
            {
                result.Add(mapper.Map());
            }

            return result;
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private void ExecuteSelectNotMap(string query)
    {
        connection.Semaphore.Wait();

        try
        {
            using NpgsqlCommand cmd = new NpgsqlCommand(query, (NpgsqlConnection)connection.Connection!);
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            ReadAllObjects(reader);
        }
        finally
        {
            connection.Semaphore.Release();
        }
    }

    private void ReadAllObjects(NpgsqlDataReader reader)
    {
        CurrentObjects.Clear();
        
        while (reader.Read())
        {
            List<FieldFromDatabase> fields = [];

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!fields.Exists(f => f.FullName == reader.GetName(i)))
                {
                    fields.Add(new FieldFromDatabase(reader.GetName(i), reader.GetFieldType(i), reader.GetValue(i), reader.GetPostgresType(i).Name));
                }
            }

            CurrentObjects.Add(new ObjectFromDatabase(fields));
        }
    }
}
