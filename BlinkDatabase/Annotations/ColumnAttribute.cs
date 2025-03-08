namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public ColumnAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}
