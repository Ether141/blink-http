using BlinkHttp.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BlinkHttp.Routing
{
    internal static class RouteUrlUtility
    {
        private static readonly Regex ValidUrlPathRegex = new Regex(@"^[a-zA-Z0-9\-._~%!$&'()*+,;=:@\/]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex RouteParameterRegex = new Regex(@"^{.*}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        internal static string GetRoutePathForController(Type controllerType, string? prefix)
        {
            string route;
            RouteAttribute? attribute = controllerType.GetCustomAttribute<RouteAttribute>();

            if (attribute != null)
            {
                route = attribute.GetRouteValue(controllerType);
            }
            else
            {
                string typeName = controllerType.Name.ToLowerInvariant();
                route = typeName.Length > 10 && typeName.EndsWith("controller") ?
                        typeName[..typeName.IndexOf("controller")] :
                        typeName;
            }

            route = AppendRoutePrefix(route, prefix);
            return route;
        }

        internal static string AppendRoutePrefix(string route, string? prefix)
        {
            if (prefix != null)
            {
                route = $"{prefix}/{route}";
            }

            return route;
        }

        internal static string GetControllerWebName(Type controllerType)
        {
            string typeName = controllerType.Name.ToLowerInvariant();
            return typeName.Length > 10 && typeName.EndsWith("controller") ?
                   typeName[..typeName.IndexOf("controller")] :
                   typeName;
        }

        internal static string RemoveQuery(string url)
        {
            int index = url.IndexOf('?');
            return index > -1 ? url[..index] : url;
        }

        internal static string RemoveRouteParameters(string url)
        {
            int index = url.IndexOf('{');
            index = index > 0 ? index - 1 : index;
            string? query = GetQuery(url);
            return index > -1 ? url[..index] + (query ?? "") : url;
        }

        internal static string? GetQuery(string url)
        {
            int index = url.IndexOf('?');
            return index > -1 ? url[index..] : null;
        }

        internal static string? GetRouteParameters(string url)
        {
            int index = url.IndexOf('{');

            if (index < 0)
            {
                return null;
            }

            url = RemoveQuery(url);
            return url[index..];
        }

        internal static bool IsValidRestApiUrl(string url) => !string.IsNullOrWhiteSpace(url) && ValidUrlPathRegex.IsMatch(url);

        internal static string TrimAndLowerUrl(string url) => url.Trim('/', '\\').Trim().ToLowerInvariant();

        internal static bool IsRouteParameter(string value) => RouteParameterRegex.IsMatch(value);
    }
}
