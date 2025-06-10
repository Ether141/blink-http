using BlinkHttp.Http;
using BlinkHttp.Logging;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class Routing : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = LoggerFactory.Create<Routing>();
    private readonly Router router;

#pragma warning disable CS8618
    public Routing(Router router)
#pragma warning restore CS8618
    {
        this.router = router;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        HttpResponse response = context.Response!;
        Http.HttpMethod? httpMethod = null;

        try
        {
            httpMethod = HttpMethodExtension.Parse(context.Request.HttpMethod);
        }
        catch
        {
            if (!context.Request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase))
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.ContentLength64 = 0;
                return;
            }
        }

        string path = context.Request!.Url!.PathAndQuery;

        if (httpMethod == null)
        {
            string? methodHeader = context.Request.Headers["Access-Control-Request-Method"];

            if (methodHeader == null)
            {
                logger.Debug($"Request is probably preflight but has not Access-Control-Request-Method header");
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.ContentLength64 = 0;
                return;
            }

            httpMethod = HttpMethodExtension.Parse(methodHeader);
        }

        Route? route = router.GetRoute(path, httpMethod.Value);

        if (route == null)
        {
            response.StatusCode = router.RouteExistsForAnyMethod(path) ? (int)HttpStatusCode.MethodNotAllowed : (int)HttpStatusCode.NotFound;
            response.ContentLength64 = 0;
            return;
        }

        if (!route.HttpMethod.ToString().Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase) && !"options".Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
        {
            logger.Debug($"Received request method is not allowed for this route. Request method: {context.Request.HttpMethod} | Required method: {route.HttpMethod}");
            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            response.ContentLength64 = 0;
            return;
        }

        context.Route = route;
        await Next(context);
    }
}
