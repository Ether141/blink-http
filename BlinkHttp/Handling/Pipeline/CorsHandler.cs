using BlinkHttp.Http;
using System.Net;
using System.Reflection;

namespace BlinkHttp.Handling.Pipeline;

internal class CorsHandler : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly CorsOptions? options;

    internal CorsHandler(CorsOptions? options)
    {
        this.options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Route!.Endpoint.MethodInfo.GetCustomAttribute<NoCorsAttribute>() != null)
        {
            await Next(context);
            return;
        }

        CorsAttribute? attr = context.Route!.Endpoint.MethodInfo.GetCustomAttribute<CorsAttribute>();

        if (options == null && attr == null)
        {
            await Next(context);
            return;
        }

        CorsOptions? opt = options;

        if (opt == null)
        {
            opt = new CorsOptions { Origin = attr!.AllowedOrigin ?? "*", Headers = attr!.AllowedHeaders ?? "*", Methods = attr!.AllowedMethods ?? "*", Credentials = attr.Credentials };
        }
        else if (attr != null)
        {
            if (attr!.AllowedOrigin != null) opt.Origin = attr.AllowedOrigin;
            if (attr!.AllowedHeaders != null) opt.Headers = attr.AllowedHeaders;
            if (attr!.AllowedMethods != null) opt.Methods = attr.AllowedMethods;
            opt.Credentials = attr!.Credentials;
        }

        context.Response.AddHeader("Access-Control-Allow-Origin", opt.Origin);
        context.Response.AddHeader("Access-Control-Allow-Headers", opt.Headers);
        context.Response.AddHeader("Access-Control-Allow-Credentials", opt.Credentials.ToString().ToLower());
        context.Response.AddHeader("Access-Control-Allow-Methods", opt.Methods);

        if (context.Request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase) && context.Request.Headers["Origin"] != null && context.Request.Headers["Access-Control-Request-Method"] != null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            return;
        }

        await Next(context);
    }
}
