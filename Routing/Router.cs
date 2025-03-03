using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Routing
{
    internal class Router : IRouter
    {
        private List<IRoutesCollection>? routes;

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

        public IEndpoint? GetEndpoint(string url)
        {
            if (routes == null)
            {
                throw new InvalidOperationException("Router is not initialized yet. Use InitializeAllRoutes(), or add single endpoint routing, before trying to obtain controller.");
            }

            url = RouteUrlUtility.TrimAndLowerUrl(url);

            foreach (IRoutesCollection controllerRoute in routes)
            {
                Route? route = controllerRoute.GetRoute(url);
                
                if (route != null)
                {
                    return route.Endpoint;
                }
            }

            return null;
        }

        public void RouteGet(string route, Func<IHttpResult> func) => Route(route, Http.HttpMethod.Get, func);

        private void Route(string route, Http.HttpMethod httpMethod, Func<IHttpResult> func)
        {
            InitializeRoutesList();
            route = RouteUrlUtility.TrimAndLowerUrl(route);
            route = RouteUrlUtility.AppendRoutePrefix(route, Options.RoutePrefix);
            routes![0].AddRoute(route, httpMethod, new EndpointDelegate(func));
        }

        private void InitializeRoutesList() => routes ??= [new SingleRoutes()];

        private void InitializeControllers()
        {
            List<Type> allControllers = RoutingReflectionUtility.GetAllControllers();

            foreach (Type controllerType in allControllers)
            {
                string route = RouteUrlUtility.GetRoutePathForController(controllerType, Options.RoutePrefix);
                Controller controller = (Controller)Activator.CreateInstance(controllerType)!;
                routes!.Add(new ControllerRoute(route, controller));
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
}
