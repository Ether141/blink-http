namespace BlinkHttp.Http;

/// <summary>
/// Indicates that parameter value will be obtained from HTTP request body, form data or raw data.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class FromBodyAttribute : Attribute { }
