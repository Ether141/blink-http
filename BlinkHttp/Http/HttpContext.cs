using BlinkHttp.Authentication;
using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Represents the context of an HTTP request and response, providing access to request and response data,
/// user information, and routing details.
/// </summary>
public sealed class HttpContext
{
    /// <summary>
    /// Gets the HTTP request associated with the current context.
    /// </summary>
    public HttpRequest Request { get; }

    /// <summary>
    /// Gets the HTTP response associated with the current context.
    /// </summary>
    public HttpResponse Response { get; }

    /// <summary>
    /// Gets or sets the buffer used for processing the response.
    /// </summary>
    public byte[]? Buffer { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the current HTTP request, typically used for authentication and authorization.
    /// </summary>
    public IUser? User { get; set; }

    internal Route? Route { get; set; }

    internal HttpContext(HttpRequest request, HttpResponse response)
    {
        Request = request;
        Response = response;
    }
}
