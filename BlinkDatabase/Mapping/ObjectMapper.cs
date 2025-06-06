using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping.Exceptions;
using BlinkDatabase.Processing;
using System.Reflection;

namespace BlinkDatabase.Mapping;

internal class ObjectMapper<T> where T : class, new()
{
    private readonly SqlMapper<T> repo;

    public ObjectMapper(SqlMapper<T> repo)
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

    private object? MapInternal(Type type, ObjectFromDatabase objectFromDatabase, Type? relationTypeToIgnore = null)
    {
        ObjectProperty[] properties = ObjectProperty.GetProperties(type);
        object obj = Activator.CreateInstance(type)!;

        foreach (ObjectProperty prop in properties)
        {
            object? value;
            FieldFromDatabase? fieldFromDb = objectFromDatabase.Fields.FirstOrDefault(o => o.FullName == prop.FullName) ?? throw new FieldNotFoundException(prop.FullName);

            if (prop.StoredType != fieldFromDb.FieldType)
            {
                value = TryGetEnum(prop, fieldFromDb);

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
                    throw new PropertyTypeMismatchException(fieldFromDb.FullName, prop.ColumnName, fieldFromDb.FieldType, prop.StoredType);
                }

                if (prop.RelationType == RelationType.OneToMany)
                {
                    HandleOneToMany(type, prop, obj, fieldFromDb);
                    continue;
                }

                value = MapInternal(prop.StoredType, objectFromDatabase);
                prop.Set(obj, value);
                continue;
            }

            value = fieldFromDb.Value;

            if (prop.IsId && fieldFromDb.Value.GetType() == typeof(DBNull))
            {
                return null;
            }

            prop.Set(obj, value);
        }

        return obj;
    }

    private T MapInternal(ObjectFromDatabase objectFromDatabases) => (T)MapInternal(typeof(T), objectFromDatabases)!;

    private void HandleOneToMany(Type type, ObjectProperty prop, object obj, FieldFromDatabase fieldFromDb)
    {
        Type constructedListType = typeof(List<>).MakeGenericType(prop.StoredType);
        System.Collections.IList objectsFromRelation = (System.Collections.IList)Activator.CreateInstance(constructedListType)!;

        List<ObjectFromDatabase> db = [.. repo.CurrentObjects.Where(o => o.Fields.Any(f => f.FullName == prop.FullName && f.Value.ToString() == fieldFromDb.Value.ToString()))];

        foreach (ObjectFromDatabase objForRelation in db)
        {
            object? o = MapInternal(prop.StoredType, objForRelation, type);

            if (o != null)
            {
                objectsFromRelation.Add(o); 
            }
        }

        foreach (object? objFromRelation in objectsFromRelation)
        {
            foreach (PropertyInfo item in prop.StoredType.GetProperties().Where(p => p.GetCustomAttribute<RelationAttribute>() != null && p.PropertyType.GetCustomAttribute<TableAttribute>()!.TableName == prop.TableName))
            {
                item.SetValue(objFromRelation, obj);
            }
        }

        prop.Set(obj, objectsFromRelation);
    }

    private static object? TryGetEnum(ObjectProperty prop, FieldFromDatabase obj)
    {
        EnumAttribute? enumAttribute = prop.StoredType.GetCustomAttribute<EnumAttribute>();
        return enumAttribute != null && enumAttribute.EnumName == obj.SqlType ? EnumMapper.TryMap(prop.StoredType, obj.Value.ToString()) : null;
    }
}
