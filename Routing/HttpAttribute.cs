using System.Reflection;

namespace BlinkHttp.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    internal abstract class HttpAttribute : Attribute
    {
        private readonly string? route;
        
        internal string? RouteParams { get; }

        internal abstract Http.HttpMethod HttpMethod { get; }

        internal HttpAttribute() { }

        internal HttpAttribute(string route)
        {
            route = RouteUrlUtility.TrimAndLowerUrl(route);
            RouteParams = RouteUrlUtility.GetRouteParameters(route);
            route = RouteUrlUtility.RemoveRouteParameters(RouteUrlUtility.RemoveQuery(route));

            if (!RouteUrlUtility.IsValidRestApiUrl(route))
            {
                throw new FormatException("Given route is in invalid format.");
            }

            this.route = route;
        }

        internal string GetRouteValue(MethodInfo methodInfo) => route ?? methodInfo.Name.ToLowerInvariant();
    }
}
