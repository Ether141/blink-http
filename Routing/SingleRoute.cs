namespace BlinkHttp.Routing
{
    internal class SingleRoute : IRoutesCollection
    {
        private Route? route;

        public IReadOnlyList<Route> Routes => route != null ? [route] : [];

        public void AddRoute(string path, Http.HttpMethod httpMethod, IEndpointMethod method)
        {
            route = new Route(path, httpMethod);
            route.CreateEndpoint(method);
        }

        public Route? GetRoute(string path) => RouteExists(path) ? route : null;

        public bool RouteExists(string path) => route != null && route.Path == path;
    }
}
