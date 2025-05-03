using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with GET method.
/// </summary>
/// <remarks>Optionally routing value can be set.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpGetAttribute : HttpAttribute
{
    /// <summary>
    /// Gets the HTTP method associated with this attribute, which is GET.
    /// </summary>
    public override HttpMethod HttpMethod => HttpMethod.Get;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class without a routing value.
    /// </summary>
    public HttpGetAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class with a specified routing value.
    /// </summary>
    /// <param name="routing">The routing value for the HTTP GET method.</param>
    public HttpGetAttribute(string routing) : base(routing) { }
}
