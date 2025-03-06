using System.Reflection;

namespace BlinkHttp.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public abstract class HttpAttribute : Attribute
{
    private readonly string? route;
    
    public abstract Http.HttpMethod HttpMethod { get; }

    internal HttpAttribute() { }

    internal HttpAttribute(string route)
    {
        route = RouteUrlUtility.TrimAndLowerUrl(route);

        if (string.IsNullOrWhiteSpace(route) || (!route.StartsWith('{') && !RouteUrlUtility.IsValidRestApiUrl(RouteUrlUtility.RemoveRouteParameters(route))))
        {
            throw new FormatException("Given route is in invalid format.");
        }

        this.route = route;
    }

    internal string GetRouteValue(MethodInfo methodInfo) 
        => route ?? (methodInfo.Name.Equals("index", StringComparison.OrdinalIgnoreCase) ? string.Empty : methodInfo.Name.ToLowerInvariant());
}
