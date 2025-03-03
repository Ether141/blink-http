using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal interface IRouter
    {
        RouterOptions? Options { get; }

        void InitializeAllRoutes();
        IEndpoint? GetEndpoint(string url);

        void RouteGet(string route, Func<IHttpResult> func);
    }
}
