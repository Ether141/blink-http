namespace BlinkHttp.Serialization.Mapping;

/// <summary>
/// An attribute used to indicate that a property is optional in the mapping of HTTP body process.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class OptionalInMappingAttribute : Attribute { }
