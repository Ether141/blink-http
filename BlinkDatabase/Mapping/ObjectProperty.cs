using BlinkDatabase.Annotations;
using System.Reflection;

namespace BlinkDatabase.Mapping;

internal class ObjectProperty
{
    private readonly PropertyInfo propertyInfo;
    private readonly ColumnAttribute columnAttribute;

    internal string ColumnName => columnAttribute.ColumnName;
    internal string TableName => propertyInfo.DeclaringType!.GetCustomAttribute<TableAttribute>()!.TableName;
    internal string FullName => $"{TableName}.{ColumnName}";
    internal Type StoredType => IsListType(propertyInfo.PropertyType) ? propertyInfo.PropertyType.GetGenericArguments()[0] : propertyInfo.PropertyType;
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

    internal object? Get(object? obj) => propertyInfo.GetValue(obj);

    internal static ObjectProperty[] GetProperties<T>() where T : class, new()
        => GetProperties(typeof(T));

    internal static ObjectProperty[] GetProperties(Type t) 
        => [.. t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<ColumnAttribute>() != null).Select(p => new ObjectProperty(p))];

    private static bool IsListType(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
}
