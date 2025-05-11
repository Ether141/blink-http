using System.Net;

namespace BlinkHttp.Server.Default;

internal static class HttpListenerContextExtension
{
    internal static HttpServerContext Wrap(this HttpListenerContext context) => 
        new HttpServerContext(new SimpleServerRequest(context.Request), new SimpleServerResponse(context.Response));
}
