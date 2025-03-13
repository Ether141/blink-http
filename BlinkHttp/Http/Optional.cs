namespace BlinkHttp.Http;

/// <summary>
/// Indicates that parameter for endpoint is optional, that is it doesn't need to be present in the request body or URL, for endpoint to be called.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class OptionalAttribute : Attribute { }