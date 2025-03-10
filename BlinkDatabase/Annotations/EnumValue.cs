using System.Reflection;

namespace BlinkDatabase.Annotations;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class EnumValueAttribute : Attribute
{
    public string Name { get; }

    public EnumValueAttribute(string name)
    {
        Name = name;
    }

    internal static string GetEnumValueName(Type enumType, object value)
    {
        Enum enumVal = (Enum)Enum.ToObject(enumType, value);
        Type type = enumVal.GetType();
        MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
        EnumValueAttribute attribute = memInfo[0].GetCustomAttribute<EnumValueAttribute>()!;
        return attribute.Name;
    }
}
