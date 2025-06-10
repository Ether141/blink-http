namespace BlinkDatabase.Annotations;

/// <summary>
/// Indicates that this enum is reflection of enum type with given name in the database.
/// </summary>
[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class EnumAttribute : Attribute
{
    /// <summary>
    /// Name of an enumeration in the database.
    /// </summary>
    public string? EnumName { get; }

    /// <summary>
    /// Creates new instance of <seealso cref="EnumAttribute"/> with specified enumeration name in the database.
    /// </summary>
    /// <param name="enumName"></param>
    public EnumAttribute(string? enumName)
    {
        EnumName = enumName;
    }
}
