using BlinkDatabase.Mapping;
using System.Data.Common;

namespace BlinkDatabase.Processing;

public class SqlMapper<T> where T : class, new()
{
    internal List<ObjectFromDatabase> CurrentObjects { get; } = [];

    private readonly ObjectMapper<T> mapper;
    private readonly Func<DbDataReader, int, string> sqlTypeNameResolver;

    public SqlMapper(Func<DbDataReader, int, string> sqlTypeNameResolver)
    {
        mapper = new ObjectMapper<T>(this);
        this.sqlTypeNameResolver = sqlTypeNameResolver;
    }

    public IEnumerable<T> MapAll(DbDataReader reader)
    {
        ReadAllObjects(reader);
        List<T> result = [];

        while (CurrentObjects.Count > 0)
        {
            result.Add(mapper.Map());
        }

        return result;
    }

    public int Count(DbDataReader reader)
    {
        ReadAllObjects(reader);
        return CurrentObjects.Count;
    }

    public void UpdateId(object? idScalar, T obj)
    {
        if (idScalar != null)
        {
            ObjectProperty idProp = ObjectProperty.GetIdProperty(typeof(T));
            idScalar = Convert.ChangeType(idScalar, idProp.StoredType);
            idProp.Set(obj, idScalar);
        }
    }

    private void ReadAllObjects(DbDataReader reader)
    {
        CurrentObjects.Clear();

        while (reader.Read())
        {
            List<FieldFromDatabase> fields = [];

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!fields.Exists(f => f.FullName == reader.GetName(i)))
                {
                    fields.Add(new FieldFromDatabase(reader.GetName(i), reader.GetFieldType(i), reader.GetValue(i), sqlTypeNameResolver.Invoke(reader, i)));
                }
            }

            CurrentObjects.Add(new ObjectFromDatabase(fields));
        }
    }
}
