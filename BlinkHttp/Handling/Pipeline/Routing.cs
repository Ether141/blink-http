using BlinkHttp.Http;
using BlinkHttp.Routing;
using Logging;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class Routing : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = Logger.GetLogger<Routing>();
    private readonly Router router;

    public Routing(Router router)
    {
        this.router = router;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        HttpResponse response = context.Response!;
        Http.HttpMethod httpMethod;

        try
        {
            httpMethod = HttpMethodExtension.Parse(context.Request.HttpMethod);
        }
        catch
        {
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            response.ContentLength64 = 0;
            return;
        }

        string path = context.Request!.Url!.PathAndQuery;
        Route? route = router.GetRoute(path, httpMethod);

        if (route == null)
        {
            response.StatusCode = router.RouteExistsForAnyMethod(path) ? (int)HttpStatusCode.MethodNotAllowed : (int)HttpStatusCode.NotFound;
            response.ContentLength64 = 0;
            return;
        }

        if (!route.HttpMethod.ToString().Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
        {
            logger.Debug($"Received request method is not allowed for this route. Request method: {route.HttpMethod} | Required method: {context.Request.HttpMethod}");
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            response.ContentLength64 = 0;
            return;
        }

        context.Route = route;
        await Next(context);
    }
}
