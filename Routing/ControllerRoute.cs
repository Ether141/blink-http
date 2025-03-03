namespace BlinkHttp.Routing
{
    internal class ControllerRoute : IRoutesCollection
    {
        internal string Path { get; }
        internal Controller Controller { get; }

        internal Type ControllerType => Controller.GetType();
        public IReadOnlyList<Route> Routes => routes;

        private readonly List<Route> routes = [];

        public ControllerRoute(string path, Controller controller)
        {
            Path = path;
            Controller = controller;
        }

        public void AddRoute(string path, Http.HttpMethod httpMethod, IEndpointMethod method)
        {
            Route route = new Route(path, httpMethod);

            if (RouteExists(route.Path))
            {
                throw new InvalidOperationException("Route with given path already exists!");
            }

            route.CreateEndpoint(this, method);
            routes.Add(route);
        }

        public bool RouteExists(string path) => routes.Any(r => r.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

        public Route? GetRoute(string path)
        {
            if (!path.StartsWith(Path))
            {
                return null;
            }

            path = path[Path.Length..].Trim('/');
            path = RouteUrlUtility.RemoveQuery(path);
            path = RouteUrlUtility.RemoveRouteParameters(path);

            return routes.FirstOrDefault(r => r.Path == path);
        }

        public override string? ToString() => $"{Path} => {ControllerType.Name}";
    }
}
