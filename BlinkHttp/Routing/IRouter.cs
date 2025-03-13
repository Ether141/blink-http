using BlinkHttp.Http;

namespace BlinkHttp.Routing;

internal interface IRouter
{
    RouterOptions? Options { get; }

    void InitializeAllRoutes(HttpContext initContext);
    Route? GetRoute(string url);
}
