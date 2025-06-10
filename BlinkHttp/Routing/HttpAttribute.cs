using System.Reflection;

namespace BlinkHttp.Routing;

/// <summary>
/// Class which is base for all Http attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public abstract class HttpAttribute : Attribute
{
    private readonly string? route;
    
    /// <summary>
    /// HTTP method which this endpoint handles.
    /// </summary>
    public abstract Http.HttpMethod HttpMethod { get; }

#pragma warning disable CS1591
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
#pragma warning restore CS1591

    internal string GetRouteValue(MethodInfo methodInfo)
        => route ?? string.Empty;// ?? (methodInfo.Name.Equals("index", StringComparison.OrdinalIgnoreCase) ? string.Empty : methodInfo.Name.ToLowerInvariant());
}
