using BlinkHttp.Http;
using BlinkHttp.Logging;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class ExceptionHandler : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = LoggerFactory.Create<ExceptionHandler>();

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception ex)
        {
            logger.Error($"Unhandled exception: {ex}");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
