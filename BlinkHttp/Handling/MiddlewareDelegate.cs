using BlinkHttp.Http;

namespace BlinkHttp.Handling;

/// <summary>
/// Represents a delegate that defines a middleware component in the HTTP pipeline.
/// </summary>
/// <param name="context">The <see cref="HttpContext"/> representing the HTTP request and response.</param>
/// <returns>A <see cref="Task"/> that represents the asynchronous operation of the middleware.</returns>
public delegate Task MiddlewareDelegate(HttpContext context);
