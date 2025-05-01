namespace BlinkHttp.Serialization.Mapping;

/// <summary>
/// An attribute used to indicate that a property should be ignored during mapping of HTTP body.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class IgnoreInMappingAttribute : Attribute { }
