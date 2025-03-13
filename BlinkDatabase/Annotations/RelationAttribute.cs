namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that value of this property should be obtained from database using relation and defined foreign key. By default foreign key is "id". This attribute must be used with <seealso cref="ColumnAttribute"/>.
/// </summary>
/// <remarks>If a type of the property marked with this attribute is <seealso cref="List{T}"/>, then this relation is treated as One To Many relation. Otherwise as One To One relation.</remarks>
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
