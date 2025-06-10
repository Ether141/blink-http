namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that property should be mapped as database column, using given column name. Every property intended to mapping, must be marked with this attribute.
/// </summary>
/// <remarks>Only properties which are members of a class marked with <seealso cref="TableAttribute"/> can use this attribute.</remarks>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ColumnAttribute : Attribute
{
    /// <summary>
    /// Name of the column in the databse.
    /// </summary>
    public string ColumnName { get; }

    /// <summary>
    /// Creates new instance of <seealso cref="ColumnAttribute"/> with specified column name in the database.
    /// </summary>
    public ColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}
