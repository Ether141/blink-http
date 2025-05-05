using BlinkHttp.Http;
using BlinkHttp.Logging;
using System.Net.Http;
using System.Reflection;

namespace BlinkHttp.Routing;

internal class Router : IRouter
{
    private List<IRoutesCollection>? routes;
    private readonly ILogger logger = LoggerFactory.Create<Router>();

    public IReadOnlyCollection<IRoutesCollection> Routes
    {
        get
        {
            ThrowIfNotInitialized();
            return routes!;
        }
    }

    public RouterOptions Options { get; } = new RouterOptions();

    public void InitializeAllRoutes()
    {
        routes ??= [];
        InitializeControllers();
        InitializeEndpoints();

        foreach (var route in routes)
        {
            Console.WriteLine(route);

            foreach (var routee in route.Routes)
            {
                Console.WriteLine($"\t{routee}");
            }
        }
    }

    public Route? GetRoute(string url, Http.HttpMethod httpMethod)
    {
        ThrowIfNotInitialized();
        url = RouteUrlUtility.TrimAndLowerUrl(url);

        foreach (IRoutesCollection routesCollection in routes!)
        {
            Route? route = routesCollection.GetRoute(url, httpMethod);
            
            if (route != null)
            {
                logger.Debug($"Matched route for {url} [{httpMethod}] => {routesCollection.ControllerPath}/{route.Path}");
                return route;
            }
        }

        logger.Debug($"Unable to find route for {url} [{httpMethod}].");
        return null;
    }

    public bool RouteExistsForAnyMethod(string url)
    {
        ThrowIfNotInitialized();
        url = RouteUrlUtility.TrimAndLowerUrl(url);
        return routes!.Any(r => r.GetRoute(url) != null);
    }

    private void ThrowIfNotInitialized()
    {
        if (routes == null)
        {
            throw new InvalidOperationException("Router is not initialized yet. Use InitializeAllRoutes() before trying to obtain route.");
        }
    }

    private void InitializeControllers()
    {
        List<Type> allControllers = RoutingReflectionUtility.GetAllControllers();

        foreach (Type controllerType in allControllers)
        {
            if (Options.IgnoredControllerTypes?.Contains(controllerType) ?? false)
            {
                continue;
            }

            string route = RouteUrlUtility.GetRoutePathForController(controllerType, Options.RoutePrefix);
            routes!.Add(new ControllerRoute(route, controllerType));
        }
    }

    private void InitializeEndpoints()
    {
        foreach (IRoutesCollection routes in routes!)
        {
            if (routes is not ControllerRoute controllerRoute)
            {
                continue;
            }

            List<MethodInfo> allMethods = RoutingReflectionUtility.GetAllEndpointMethods(controllerRoute.ControllerType);
            
            foreach (MethodInfo methodInfo in allMethods)
            {
                HttpAttribute attribute = methodInfo.GetCustomAttribute<HttpAttribute>()!;
                controllerRoute.AddRoute(attribute.GetRouteValue(methodInfo), attribute.HttpMethod, methodInfo);
            }
        }
    }
}
