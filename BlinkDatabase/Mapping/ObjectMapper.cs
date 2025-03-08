using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping.Exceptions;
using BlinkDatabase.PostgreSql;
using System.Reflection;

namespace BlinkDatabase.Mapping;

internal class ObjectMapper<T> where T : class, new()
{
    private readonly PostgreSqlRepository<T> repo;

    public ObjectMapper(PostgreSqlRepository<T> repo)
    {
        this.repo = repo;
    }

    internal T Map()
    {
        ObjectFromDatabase obj = repo.CurrentObjects[0];
        T result = MapInternal(obj);
        repo.CurrentObjects.RemoveAll(o => o.Id == obj.Id);
        return result;
    }

    private object MapInternal(Type type, ObjectFromDatabase objectFromDatabase, Type? relationTypeToIgnore = null)
    {
        ObjectProperty[] properties = ObjectProperty.GetProperties(type);
        object obj = Activator.CreateInstance(type)!;

        foreach (ObjectProperty prop in properties)
        {
            object? value;
            FieldFromDatabase? dbObj = objectFromDatabase.Fields.FirstOrDefault(o => o.FullName == prop.FullName) ?? throw new PropertyNotFoundException(prop.FullName);

            if (prop.StoredType != dbObj.FieldType)
            {
                value = TryGetEnum(prop, dbObj);

                if (value != null)
                {
                    prop.Set(obj, value);
                    continue;
                }

                if (relationTypeToIgnore == prop.StoredType)
                {
                    continue;
                }

                if (!prop.IsRelation)
                {
                    throw new PropertyTypeMismatchException(dbObj.FullName, prop.ColumnName, dbObj.FieldType, prop.StoredType);
                }

                if (prop.RelationType == RelationType.OneToMany)
                {
                    Type constructedListType = typeof(List<>).MakeGenericType(prop.StoredType);
                    System.Collections.IList objectsFromRelation = (System.Collections.IList)Activator.CreateInstance(constructedListType)!;

                    ObjectFromDatabase? objFromDb = objectFromDatabase;
                    List<ObjectFromDatabase> db = [.. repo.CurrentObjects.Where(o => o.Fields.Any(f => f.FullName == prop.FullName && f.Value.ToString() == dbObj.Value.ToString()))];

                    foreach (ObjectFromDatabase objForRelation in db)
                    {
                        object o = MapInternal(prop.StoredType, objForRelation, type);
                        objectsFromRelation.Add(o);
                    }

                    foreach (object? objFromRelation in objectsFromRelation)
                    {
                        foreach (PropertyInfo item in prop.StoredType.GetProperties().Where(p => p.GetCustomAttribute<RelationAttribute>() != null && p.PropertyType.GetCustomAttribute<TableAttribute>()!.TableName == prop.TableName))
                        {
                            item.SetValue(objFromRelation, obj);
                        }
                    }

                    prop.Set(obj, objectsFromRelation);
                    continue;
                }

                value = MapInternal(prop.StoredType, objectFromDatabase);
                prop.Set(obj, value);
                continue;
            }

            value = dbObj.Value;
            prop.Set(obj, value);
        }

        return obj;
    }

    private T MapInternal(ObjectFromDatabase objectFromDatabases) => (T)MapInternal(typeof(T), objectFromDatabases);

    private static object? TryGetEnum(ObjectProperty prop, FieldFromDatabase obj)
    {
        EnumAttribute? enumAttribute = prop.StoredType.GetCustomAttribute<EnumAttribute>();
        return enumAttribute != null && enumAttribute.EnumName == obj.PgsqlType ? (Enum.TryParse(prop.StoredType, obj.Value.ToString(), true, out object? result) ? result : null) : null;
    }

    //internal static NpgsqlCommand MapInsert<T>(T obj) where T : class, new()
    //{
    //    ObjectProperty[] properties = ObjectProperty.GetProperties<T>();
    //    TableAttribute tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>() ?? throw new TableAttributeMissingException(typeof(T).Name);

    //    StringBuilder valueNamesBuilder = new StringBuilder();
    //    StringBuilder valuesBuilder = new StringBuilder();
    //    List<(string, object?)> values = [];

    //    for (int i = 0; i < properties.Length; i++)
    //    {
    //        ObjectProperty property = properties[i];
    //        string columnName = property.ColumnName;
    //        valueNamesBuilder.Append(columnName);
    //        valuesBuilder.Append($"@{columnName}");
    //        values.Add((columnName, property.Get(obj)));

    //        if (i < properties.Length - 1)
    //        {
    //            valueNamesBuilder.Append(", ");
    //            valuesBuilder.Append(", ");
    //        }
    //    }

    //    string tableName = tableAttribute.TableName;
    //    string cmdText = $"INSERT INTO {tableName} ({valueNamesBuilder}) VALUES ({valuesBuilder})";
    //    NpgsqlCommand cmd = new NpgsqlCommand(cmdText);

    //    foreach ((string name, object? value) in values)
    //    {
    //        cmd.Parameters.AddWithValue(name, value!);
    //    }

    //    return cmd;
    //}
}
