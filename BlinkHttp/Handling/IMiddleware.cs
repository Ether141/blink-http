using BlinkHttp.Handling.Pipeline;
using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Handling;

/// <summary>
/// Represents the base class for middleware components that can handle HTTP requests and responses.
/// </summary>
public interface IMiddleware
{
    MiddlewareDelegate Next { get; set; }

    Task InvokeAsync(HttpContext context);
}
