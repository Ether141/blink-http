using BlinkDatabase.Annotations;
using System.Reflection;

namespace BlinkDatabase.Mapping;

internal class ObjectProperty
{
    private readonly PropertyInfo propertyInfo;
    private readonly ColumnAttribute columnAttribute;

    internal string Name => propertyInfo.Name;
    internal string ColumnName => columnAttribute.ColumnName;
    internal string TableName => propertyInfo.DeclaringType!.GetCustomAttribute<TableAttribute>()!.TableName;
    internal string FullName => $"{TableName}.{ColumnName}";
    internal Type StoredType => IsListType(propertyInfo.PropertyType) ? propertyInfo.PropertyType.GetGenericArguments()[0] : 
                                (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)! : propertyInfo.PropertyType);
    internal bool IsId => propertyInfo.GetCustomAttribute<IdAttribute>() != null;
    internal bool IsRelation => propertyInfo.GetCustomAttribute<RelationAttribute>() != null;
    internal RelationType? RelationType => IsRelation ? (IsListType(propertyInfo.PropertyType) ? Mapping.RelationType.OneToMany : Mapping.RelationType.OneToOne) : null;
    internal string? RelationTableName => IsRelation ? StoredType.GetCustomAttribute<TableAttribute>()!.TableName : null;
    internal string? RelationColumnName => IsRelation ? propertyInfo.GetCustomAttribute<RelationAttribute>()!.ColumnName : null;

    internal ObjectProperty(PropertyInfo propertyInfo)
    {
        columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>()!;
        this.propertyInfo = propertyInfo;
    }

    internal void Set(object? obj, object? value) => propertyInfo.SetValue(obj, value);

    internal object? Get(object obj) => propertyInfo.GetValue(obj);

    internal string GetAsSqlString(object obj)
    {
        object? value = Get(obj);
        Type type = value!.GetType();

        if (value == null)
        {
            return "NULL";
        }
        else if (type == typeof(string))
        {
            string val = ((string)value).Replace("'", "''");
            return $"'{val}'";
        }
        else if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
        {
            return value.ToString()!.Replace(',', '.');
        }
        else if (type.IsEnum)
        {
            return $"'{EnumValueAttribute.GetEnumValueName(type, value)}'";
        }
        else if (type == typeof(DateTime))
        {
            DateTime dateTime = (DateTime)value;

            if (dateTime.TimeOfDay.Ticks > 0)
            {
                return $"'{(DateTime)value:yyyy-MM-dd HH:mm:ss.fff}'";
            }
            else
            {
                return $"'{(DateTime)value:yyyy-MM-dd}'";
            }
        }
        else
        {
            return value.ToString() ?? "NULL";
        }
    }

    internal static ObjectProperty[] GetProperties<T>() where T : class, new()
    => GetProperties(typeof(T));

    internal static ObjectProperty[] GetProperties(Type t)
        => [.. t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<ColumnAttribute>() != null).Select(p => new ObjectProperty(p))];

    internal static ObjectProperty GetIdProperty(Type t) => new ObjectProperty(t.GetProperties(BindingFlags.Public | BindingFlags.Instance).First(p => p.GetCustomAttribute<IdAttribute>() != null));

    private static bool IsListType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
}
