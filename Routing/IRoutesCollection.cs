namespace BlinkHttp.Routing
{
    internal interface IRoutesCollection
    {
        IReadOnlyList<Route> Routes { get; }

        void AddRoute(string path, Http.HttpMethod httpMethod, IEndpointMethod method);
        bool RouteExists(string path);
        Route? GetRoute(string path);
    }
}
