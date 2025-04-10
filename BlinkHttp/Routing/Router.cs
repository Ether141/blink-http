﻿using BlinkHttp.Http;
using Logging;
using System.Reflection;

namespace BlinkHttp.Routing;

internal class Router : IRouter
{
    private List<IRoutesCollection>? routes;
    private readonly ILogger logger = Logger.GetLogger<Router>();

    public RouterOptions Options { get; }

    public Router()
    {
        Options = new RouterOptions();
    }

    public void InitializeAllRoutes()
    {
        InitializeRoutesList();
        InitializeControllers();
        InitializeEndpoints();
    }

    public Route? GetRoute(string url)
    {
        if (routes == null)
        {
            throw new InvalidOperationException("Router is not initialized yet. Use InitializeAllRoutes(), or add single endpoint routing, before trying to obtain controller.");
        }

        url = RouteUrlUtility.TrimAndLowerUrl(url);

        foreach (IRoutesCollection routesCollection in routes)
        {
            Route? route = routesCollection.GetRoute(url);
            
            if (route != null)
            {
                logger.Debug($"Matched route for {url} => {routesCollection.Path}/{route.Path}");
                return route;
            }
        }

        logger.Debug($"Unable to find route for {url}.");
        return null;
    }

    private void InitializeRoutesList() => routes ??= [];

    private void InitializeControllers()
    {
        List<Type> allControllers = RoutingReflectionUtility.GetAllControllers();

        foreach (Type controllerType in allControllers)
        {
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
                controllerRoute.AddRoute(attribute.GetRouteValue(methodInfo), attribute.HttpMethod, new EndpointMethod(methodInfo));
            }
        }
    }
}
