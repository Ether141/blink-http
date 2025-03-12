using BlinkHttp.Http;
using BlinkHttp.Serialization;
using System.Reflection;

namespace BlinkHttp.Routing;

internal class ControllerRoute : IRoutesCollection
{
    public string Path { get; }
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

        if (!GetRequestParameters.CompareArgumentsAndParametersCount(route, route.Endpoint.Method.MethodInfo))
        {
            throw new ArgumentException("Method required parameters count is not the same as route parameters.");
        }

        if (!ValidateOptionalAttributes(route.Endpoint.Method.MethodInfo))
        {
            throw new ArgumentException("Optional attribute cannot be used without any From* attribute.");
        }

        if (!ValidateAttributes(route.Endpoint.Method.MethodInfo))
        {
            throw new ArgumentException("Atrribute FromQuery cannot be combined with FromForm.");
        }

        if (!ValidateAttributesOrder(route.Endpoint.Method.MethodInfo))
        {
            throw new ArgumentException("Atrributes FromForm must be defined after all FromQuery attributes.");
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

    private static bool ValidateOptionalAttributes(MethodInfo methodInfo)
        => methodInfo.GetParameters().Where(p => p.GetCustomAttribute<OptionalAttribute>() != null).All(p => p.GetCustomAttribute<FromQueryAttribute>() != null || p.GetCustomAttribute<FromBodyAttribute>() != null);

    private static bool ValidateAttributes(MethodInfo methodInfo)
        => !methodInfo.GetParameters().Where(p => p.GetCustomAttribute<FromQueryAttribute>() != null && p.GetCustomAttribute<FromBodyAttribute>() != null).Any();

    private static bool ValidateAttributesOrder(MethodInfo methodInfo)
    {
        ParameterInfo[] allParameters = methodInfo.GetParameters();

        if (allParameters.All(p => p.GetCustomAttribute<FromQueryAttribute>() == null) || allParameters.All(p => p.GetCustomAttribute<FromBodyAttribute>() == null))
        {
            return true;
        }

        bool wasBodyParameter = false;

        foreach (ParameterInfo para in allParameters)
        {
            if (para.GetCustomAttribute<FromBodyAttribute>() != null)
            {
                wasBodyParameter = true;
            }

            if (wasBodyParameter && para.GetCustomAttribute<FromQueryAttribute>() != null)
            {
                return false;
            }
        }

        return true;
    }
}
