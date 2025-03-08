using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using BlinkDatabase.Sql;
using Npgsql;
using System.Linq.Expressions;
using System.Reflection;

namespace BlinkDatabase.PostgreSql;

public class PostgreSqlRepository<T> where T : class, new()
{
    public string TableName { get; }
    internal List<ObjectFromDatabase> CurrentObjects { get; } = [];

    private readonly ObjectMapper<T> mapper;
    private readonly NpgsqlConnection connection;

    public PostgreSqlRepository(NpgsqlConnection connection)
    {
        this.connection = connection;

        TableName = typeof(T).GetCustomAttribute<TableAttribute>()!.TableName;
        mapper = new ObjectMapper<T>(this);
    }

    public IEnumerable<T> Select()
    {
        string query = SqlSelectBuilder.SelectAll<T>();
        Console.WriteLine(query);
        return ExecuteSelect(query);
    }

    public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
    {
        string query = SqlSelectBuilder.SelectWhere(expression);
        Console.WriteLine(query);
        return ExecuteSelect(query);
    }

    private IEnumerable<T> ExecuteSelect(string query)
    {
        using NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
        using NpgsqlDataReader reader = cmd.ExecuteReader();
        List<T> result = [];
        ReadAllObjects(reader);

        while (CurrentObjects.Count > 0)
        {
            result.Add(mapper.Map());
        }

        return result;
    }

    private void ReadAllObjects(NpgsqlDataReader reader)
    {
        CurrentObjects.Clear();

        while (reader.Read())
        {
            List<FieldFromDatabase> fields = [];

            for (int i = 0; i < reader.FieldCount; i++)
            {
                fields.Add(new FieldFromDatabase(reader.GetName(i), reader.GetFieldType(i), reader.GetValue(i), reader.GetPostgresType(i).Name));
            }
            
            CurrentObjects.Add(new ObjectFromDatabase(fields));
        }
    }
}
