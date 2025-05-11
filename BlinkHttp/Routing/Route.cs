using System.Reflection;

namespace BlinkHttp.Routing;

internal class Route
{
    internal string Path { get; }
    internal string? QueryString { get; }
    internal string PathWithQuery => Path + QueryString;
    internal Http.HttpMethod HttpMethod { get; }

    internal ControllerRoute AssociatedRoute { get; private set; }
    internal IEndpoint Endpoint { get; private set; }

    internal bool HasRouteParameters => Path.Split('/', StringSplitOptions.RemoveEmptyEntries).Any(RouteUrlUtility.IsRouteParameter);

    internal Route(string path, Http.HttpMethod httpMethod, ControllerRoute associatedRoute, MethodInfo endpointMethodInfo)
    {
        Path = RouteUrlUtility.RemoveQuery(path);
        QueryString = RouteUrlUtility.GetQuery(path);
        HttpMethod = httpMethod;
        AssociatedRoute = associatedRoute;
        Endpoint = new ControllerEndpoint(HttpMethod, associatedRoute.ControllerType, endpointMethodInfo);
    }

    internal bool CanRoute(string route)
    {
        route = RouteUrlUtility.RemoveQuery(route);
        string path = RouteUrlUtility.RemoveQuery(Path);

        string[] pathSplit = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
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

    internal bool CanRoute(string route, Http.HttpMethod httpMethod)
    {
        if (HttpMethod != httpMethod)
        {
            return false;
        }

        return CanRoute(route);
    }

    public override string? ToString() => $"[{HttpMethod}] {Path} {QueryString}";
}
