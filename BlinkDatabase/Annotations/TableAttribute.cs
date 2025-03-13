using System.Reflection;

namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that class is a reflection of a table with given name in the database.
/// </summary>
/// <remarks>In this class, each property which is mapping of a column in the database, must be marked with <seealso cref="ColumnAttribute"/>, and exactly one of them must be marked with <seealso cref="IdAttribute"/>.</remarks>
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
