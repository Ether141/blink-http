namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that property is primary key in the reflected table. This attribute must be used only once in the whole class and used with <seealso cref="ColumnAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class IdAttribute : Attribute { }