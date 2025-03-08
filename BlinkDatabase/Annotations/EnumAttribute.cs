namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class EnumAttribute : Attribute
{
    public string? EnumName { get; }

    public EnumAttribute(string? enumName)
    {
        EnumName = enumName;
    }
}
