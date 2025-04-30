using BlinkHttp.Http;
using Logging;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class ExceptionHandler : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = Logger.GetLogger<ExceptionHandler>();

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
