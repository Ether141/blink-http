namespace BlinkHttp.Routing
{
    internal class SingleRoutes : IRoutesCollection
    {
        public IReadOnlyList<Route> Routes => routes;

        private readonly List<Route> routes = [];

        public void AddRoute(string path, Http.HttpMethod httpMethod, IEndpointMethod method)
        {
            Route route = new Route(path, httpMethod);

            if (RouteExists(route.Path))
            {
                throw new InvalidOperationException("Route with given path already exists!");
            }

            route.CreateEndpoint(method);
            routes.Add(route);
        }

        public bool RouteExists(string path) => routes.Any(r => r.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        public Route? GetRoute(string path) => routes.FirstOrDefault(r => r.Path == path);
    }
}
