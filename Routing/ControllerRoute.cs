using BlinkHttp.Serialization;

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
                throw new InvalidOperationException("Route with given path already exists.");
            }

            route.CreateEndpoint(this, method);

            if (!GetRequestParameters.CompareArgumentsAndParametersCount(route, ((EndpointMethod)route.Endpoint!.Method).MethodInfo))
            {
                throw new ArgumentException("Method required parameters count is not the same as route parameters.");
            }

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
            return routes.FirstOrDefault(r => r.CanRoute(path));
        }

        public override string? ToString() => $"{Path} => {ControllerType.Name}";
    }
}
