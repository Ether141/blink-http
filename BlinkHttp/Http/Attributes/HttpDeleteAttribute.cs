using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with DELETE method.
/// </summary>
/// <remarks>Optionally routing value can be set.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpDeleteAttribute : HttpAttribute
{
    /// <summary>
    /// Gets the HTTP method associated with this attribute, which is DELETE.
    /// </summary>
    public override HttpMethod HttpMethod => HttpMethod.Delete;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpDeleteAttribute"/> class without a routing value.
    /// </summary>
    public HttpDeleteAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpDeleteAttribute"/> class with a specified routing value.
    /// </summary>
    /// <param name="routing">The routing value for the DELETE endpoint.</param>
    public HttpDeleteAttribute(string routing) : base(routing) { }
}
