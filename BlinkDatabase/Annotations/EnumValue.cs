namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class EnumValueAttribute : Attribute
{
    public string Name { get; }

    public EnumValueAttribute(string name)
    {
        Name = name;
    }
}
