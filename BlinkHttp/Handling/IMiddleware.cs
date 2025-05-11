using BlinkHttp.Handling.Pipeline;
using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Handling;

/// <summary>
/// Defines a middleware component that can process HTTP requests asynchronously.
/// </summary>
public interface IMiddleware
{
    /// <summary>
    /// Gets or sets the next middleware component in the pipeline.
    /// </summary>
    MiddlewareDelegate Next { get; set; }

    /// <summary>
    /// Invokes the middleware to process the specified HTTP context.
    /// </summary>
    /// <param name="context">The HTTP context containing request and response information.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InvokeAsync(HttpContext context);
}
