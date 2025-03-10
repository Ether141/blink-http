using System.Reflection;

namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TableAttribute : Attribute
{
    public string TableName { get; }

    public TableAttribute(string tableName)
    {
        TableName = tableName;
    }

    internal static string GetTableName<T>() where T : class, new() => typeof(T).GetCustomAttribute<TableAttribute>()!.TableName;
}
