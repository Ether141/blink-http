namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TableAttribute : Attribute
{
    public string TableName { get; }

    public TableAttribute(string tableName)
    {
        TableName = tableName;
    }
}
