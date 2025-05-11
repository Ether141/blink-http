using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with POST method.
/// </summary>
/// <remarks>Optionally routing value can be set.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpPostAttribute : HttpAttribute
{
    /// <summary>
    /// Gets the HTTP method associated with this attribute, which is POST.
    /// </summary>
    public override HttpMethod HttpMethod => HttpMethod.Post;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class.
    /// </summary>
    public HttpPostAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class with the specified routing value.
    /// </summary>
    /// <param name="routing">The routing value for the HTTP POST endpoint.</param>
    public HttpPostAttribute(string routing) : base(routing) { }
}
