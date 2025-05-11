using BlinkHttp.Http;
using BlinkHttp.Serialization;
using System.Reflection;

namespace BlinkHttp.Routing;

internal class ControllerRoute : IRoutesCollection
{
    public string ControllerPath { get; }
    public IReadOnlyList<Route> Routes => routes;

    internal Type ControllerType { get; }

    private readonly List<Route> routes = [];

    public ControllerRoute(string controllerPath, Type controllerType)
    {
        ControllerPath = controllerPath;
        ControllerType = controllerType;
    }

    public void AddRoute(string pathWithQuery, Http.HttpMethod httpMethod, MethodInfo endpointMethod)
    {
        if (RouteExists(pathWithQuery, httpMethod))
        {
            throw new InvalidOperationException($"Route with given path already exists: [{httpMethod}] {pathWithQuery}");
        }

        Route route = new Route(pathWithQuery, httpMethod, this, endpointMethod);

        if (!GetRequestParameters.CompareArgumentsAndParametersCount(route, route.Endpoint.MethodInfo))
        {
            throw new ArgumentException($"Method required parameters count is not the same as route parameters. {route.Endpoint.MethodInfo.Name}");
        }

        if (!ValidateOptionalAttributes(route.Endpoint.MethodInfo))
        {
            throw new ArgumentException("Optional attribute cannot be used without any From* attribute.");
        }

        if (!ValidateAttributes(route.Endpoint.MethodInfo))
        {
            throw new ArgumentException("Atrribute FromQuery cannot be combined with FromForm.");
        }

        if (!ValidateAttributesOrder(route.Endpoint.MethodInfo))
        {
            throw new ArgumentException("Atrributes FromForm must be defined after all FromQuery attributes.");
        }

        routes.Add(route);
    }

    public bool RouteExists(string path, Http.HttpMethod method) => routes.Any(r => r.Path.Equals(path, StringComparison.OrdinalIgnoreCase) && r.HttpMethod == method);

    public Route? GetRoute(string path, Http.HttpMethod method)
    {
        if (!path.StartsWith(ControllerPath))
        {
            return null;
        }

        path = path[ControllerPath.Length..].Trim('/');
        return routes.FirstOrDefault(r => r.CanRoute(path, method));
    }

    public Route? GetRoute(string path)
    {
        if (!path.StartsWith(ControllerPath))
        {
            return null;
        }

        path = path[ControllerPath.Length..].Trim('/');
        return routes.FirstOrDefault(r => r.CanRoute(path));
    }

    public override string? ToString() => $"{ControllerPath} => {ControllerType.Name}";

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
