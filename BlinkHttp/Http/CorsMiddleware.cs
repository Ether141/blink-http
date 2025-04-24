using BlinkHttp.Handling;
using System.Net;

namespace BlinkHttp.Http;

internal class CorsMiddleware : IMiddleware
{
    private readonly CorsOptions options;

    internal CorsMiddleware(CorsOptions options)
    {
        this.options = options;
    }

    public bool Handle(HttpListenerRequest request, HttpListenerResponse response)
    {
        response.AddHeader("Access-Control-Allow-Origin", options.Origin);
        response.AddHeader("Access-Control-Allow-Headers", options.Headers);
        response.AddHeader("Access-Control-Allow-Credentials", options.Credentials.ToString().ToLower());
        response.AddHeader("Access-Control-Allow-Methods", options.Methods);

        if (request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase) && request.Headers["Origin"] != null && request.Headers["Access-Control-Request-Method"] != null)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            return false;
        }

        return true;
    }
}
