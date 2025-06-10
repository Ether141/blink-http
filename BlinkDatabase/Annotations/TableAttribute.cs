using System.Reflection;

namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that class is a reflection of a table with given name in the database.
/// </summary>
/// <remarks>In this class, each property which is mapping of a column in the database, must be marked with <seealso cref="ColumnAttribute"/>, and exactly one of them must be marked with <seealso cref="IdAttribute"/>.</remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TableAttribute : Attribute
{
    /// <summary>
    /// Name of a table in the database, which will be used to obtain data for model.
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// Creates new instance of <seealso cref="TableAttribute"/> with specified table name.
    /// </summary>
    public TableAttribute(string tableName)
    {
        TableName = tableName;
    }

    internal static string GetTableName<T>() where T : class, new() => typeof(T).GetCustomAttribute<TableAttribute>()!.TableName;
}
