using BlinkHttp.Http;

namespace BlinkHttp.Handling;

public delegate Task MiddlewareDelegate(HttpContext context);
