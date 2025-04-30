using BlinkHttp.Handling;
using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class CorsHandler : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly CorsOptions options;

    internal CorsHandler(CorsOptions options)
    {
        this.options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.AddHeader("Access-Control-Allow-Origin", options.Origin);
        context.Response.AddHeader("Access-Control-Allow-Headers", options.Headers);
        context.Response.AddHeader("Access-Control-Allow-Credentials", options.Credentials.ToString().ToLower());
        context.Response.AddHeader("Access-Control-Allow-Methods", options.Methods);

        if (context.Request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase) && context.Request.Headers["Origin"] != null && context.Request.Headers["Access-Control-Request-Method"] != null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            return;
        }

        await Next(context);
    }
}
