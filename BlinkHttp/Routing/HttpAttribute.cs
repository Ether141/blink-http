using System.Reflection;

namespace BlinkHttp.Routing;

/// <summary>
/// Class which is base for all Http attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public abstract class HttpAttribute : Attribute
{
    private readonly string? route;
    
    public abstract Http.HttpMethod HttpMethod { get; }

    protected HttpAttribute() { }

    protected HttpAttribute(string route)
    {
        route = RouteUrlUtility.TrimAndLowerUrl(route);

        if (string.IsNullOrWhiteSpace(route))
        {
            throw new FormatException("Given route is in invalid format.");
        }

        this.route = route;
    }

    internal string GetRouteValue(MethodInfo methodInfo)
        => route ?? string.Empty;// ?? (methodInfo.Name.Equals("index", StringComparison.OrdinalIgnoreCase) ? string.Empty : methodInfo.Name.ToLowerInvariant());
}
