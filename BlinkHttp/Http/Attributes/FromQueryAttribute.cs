namespace BlinkHttp.Http;

/// <summary>
/// Indicates that parameter value will be obtained from request URL, from query string or route.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class FromQueryAttribute : Attribute { }
