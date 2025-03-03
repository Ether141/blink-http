namespace BlinkHttp.Routing
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class RouteAttribute : Attribute
    {
        private readonly string route;

        internal RouteAttribute(string route)
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
}
