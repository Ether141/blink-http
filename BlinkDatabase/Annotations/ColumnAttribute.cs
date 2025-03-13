namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that property should be mapped as database column, using given column name. Every property intended to mapping, must be marked with this attribute.
/// </summary>
/// <remarks>Only properties which are members of a class marked with <seealso cref="TableAttribute"/> can use this attribute.</remarks>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public ColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}
