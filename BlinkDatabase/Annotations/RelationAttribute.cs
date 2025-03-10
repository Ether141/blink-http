namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class RelationAttribute : Attribute 
{
    public string ColumnName { get; }

    public RelationAttribute()
    {
        ColumnName = "id";
    }

    public RelationAttribute(string columnName)
    {
        ColumnName = columnName;
    }
}
