namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that this enum is reflection of enum type with given name in the database.
/// </summary>
[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class EnumAttribute : Attribute
{
    public string? EnumName { get; }

    public EnumAttribute(string? enumName)
    {
        EnumName = enumName;
    }
}
