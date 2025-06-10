using BlinkHttp.Http;
using BlinkHttp.Logging;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class ExceptionHandler : IMiddleware
{
#pragma warning disable CS8618
    public MiddlewareDelegate Next { get; set; }
#pragma warning restore CS8618

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
