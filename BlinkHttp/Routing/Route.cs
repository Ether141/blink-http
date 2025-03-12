namespace BlinkHttp.Routing;

internal class Route
{
    internal string Path { get; }
    internal Http.HttpMethod HttpMethod { get; }

    internal ControllerRoute? AssociatedRoute { get; private set; }
    internal IEndpoint Endpoint { get; private set; }

    internal bool HasRouteParameters => Path.Split('/', StringSplitOptions.RemoveEmptyEntries).Any(RouteUrlUtility.IsRouteParameter);

    internal Route(string path, Http.HttpMethod httpMethod)
    {
        Path = path;
        HttpMethod = httpMethod;
    }

    internal void CreateEndpoint(ControllerRoute associatedRoute, IEndpointMethod endpointMethod)
    {
        AssociatedRoute = associatedRoute;
        Endpoint = new ControllerEndpoint(HttpMethod, associatedRoute.Controller, endpointMethod);
    }

    internal bool CanRoute(string route)
    {
        route = RouteUrlUtility.RemoveQuery(route);

        string[] pathSplit = Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string[] routeSplit = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (pathSplit.Length != routeSplit.Length)
        {
            return false;
        }

        for (int i = 0; i < pathSplit.Length; i++)
        {
            if (!RouteUrlUtility.IsRouteParameter(pathSplit[i]) && !pathSplit[i].Equals(routeSplit[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override string? ToString() => $"[{HttpMethod}] {Path}";
}
