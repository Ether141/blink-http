using BlinkHttp.Authentication;
using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Basic class for all server controllers. It contains endpoints, that can be used in HTTP requests.
/// </summary>
public abstract class Controller
{
    /// <summary>
    /// Currently handled request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerRequest? Request { get; internal set; }

    /// <summary>
    /// Response that will be sent to the user, after complete handling of request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerResponse? Response { get; internal set; }

    /// <summary>
    /// Information about user that is associated with current request. Available only from endpoint method, marked with <seealso cref="AuthorizeAttribute"/> and when authorization was turned on during building <seealso cref="BlinkHttp.Application.WebApplication"/>.
    /// </summary>
    public IUser? User { get; internal set; }

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 200 OK, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult Ok() => JsonResult.FromObject(JsonContent(200, "OK"));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 201 Created, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult Created() => JsonResult.FromObject(JsonContent(201, "Created"));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 404 Not Found, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult NotFound() => JsonResult.FromObject(JsonContent(404, "Requested resource cannot be found."));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 400 Bad Request, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult BadRequest() => JsonResult.FromObject(JsonContent(400, "Bad request."));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 401 Unauthorized, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult Unauthorized() => JsonResult.FromObject(JsonContent(401, "Unauthorized"));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 403 Forbidden, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult Forbidden() => JsonResult.FromObject(JsonContent(403, "Forbidden"));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 409 Conflict, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult Conflict() => JsonResult.FromObject(JsonContent(409, "Conflict encountered."));

    /// <summary>
    /// Returns <seealso cref="IHttpResult"/> with HTTP code 500 Internal Server Error, and JSON object with status code and appropriate message.
    /// </summary>
    protected IHttpResult InternalServerError() => JsonResult.FromObject(JsonContent(500, "Internal Server Error."));

    private static object JsonContent(int status, string message) => new { status, message };
}
