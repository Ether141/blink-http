using BlinkHttp.Http;

namespace BlinkHttp.Routing;

internal interface IRouter
{
    IReadOnlyCollection<IRoutesCollection> Routes { get; }
    RouterOptions? Options { get; }

    void InitializeAllRoutes();
    Route? GetRoute(string url, Http.HttpMethod httpMethod);
}
