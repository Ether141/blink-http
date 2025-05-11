using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Defines route for controller. Later, this route will be used in conjunction with the route of each endpoint, at the beginning of the entire route.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class RouteAttribute : Attribute
{
    private readonly string route;

    public RouteAttribute(string route)
    {
        this.route = ProcessRouteUrl(route);   
    }

    internal string GetRouteValue(Type controllerType)
    {
        if (!controllerType.IsAssignableTo(typeof(Controller)))
        {
            throw new InvalidOperationException("Given type does not derive from Controller class.");
        }

        return !route.Contains("[controller]", StringComparison.OrdinalIgnoreCase) ? route : route.Replace("[controller]", RouteUrlUtility.GetControllerWebName(controllerType));
    }

    private static string ProcessRouteUrl(string url)
    {
        url = RouteUrlUtility.TrimAndLowerUrl(url);

        string toValidate = !url.Contains("[controller]", StringComparison.OrdinalIgnoreCase) ? url : url.Replace("[controller]", string.Empty);

        if (!string.IsNullOrEmpty(toValidate) && !RouteUrlUtility.IsValidRestApiUrl(toValidate))
        {
            throw new FormatException("Given route URL is in invalid format.");
        }

        return url;
    }
}
