using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with PUT method.
/// </summary>
/// <remarks>Optionally routing value can be set.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpPutAttribute : HttpAttribute
{
    /// <summary>
    /// Gets the HTTP method associated with this attribute, which is PUT.
    /// </summary>
    public override HttpMethod HttpMethod => HttpMethod.Put;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class without specifying a routing value.
    /// </summary>
    public HttpPutAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpPutAttribute"/> class with the specified routing value.
    /// </summary>
    /// <param name="routing">The routing value for the PUT endpoint.</param>
    public HttpPutAttribute(string routing) : base(routing) { }
}
