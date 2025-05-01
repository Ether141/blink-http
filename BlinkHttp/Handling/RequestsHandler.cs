using BlinkHttp.Routing;
using BlinkHttp.Handling.Pipeline;
using BlinkHttp.Authentication;
using BlinkHttp.Server;
using BlinkHttp.Configuration;
using BlinkHttp.Http;
using System.Net;
using Logging;

namespace BlinkHttp.Handling;

internal class RequestsHandler
{
    private readonly Router router;
    private readonly HttpPipeline pipeline;

    private readonly ILogger logger = Logger.GetLogger<RequestsHandler>();

    public RequestsHandler(IAuthorizer? authorizer, IMiddleware[] middlewares, string? routePrefix)
    {
        router = ConfigureRouter(routePrefix);
        pipeline = PipelineBuilder.GetPipelineBuilderWithDefaults(router, authorizer, middlewares).Build();
    }

    internal async Task HandleRequestAsync(HttpServerContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        HttpContext _context = new HttpContext(request, response);

        logger.Debug($"Received request [{request.HttpMethod}] from {request.LocalEndPoint.Address} - {request.Url}");

        await pipeline.Invoke(_context);

        if (_context.Buffer != null)
        {
            using Stream output = response.OutputStream;
            response.ContentLength64 = _context.Buffer.Length;
            await output.WriteAsync(_context.Buffer);
        }

        response.Close();
        logger.Debug($"Handling request finished with status code: {response.StatusCode}. Response size: {_context.Buffer?.Length ?? 0}");
    }

    private Router ConfigureRouter(string? routePrefix)
    {
        Router router = new Router();
        router.Options.RoutePrefix = routePrefix;
        router.InitializeAllRoutes();
        return router;
    }
}
