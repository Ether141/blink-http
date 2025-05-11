using BlinkHttp.Http;

namespace BlinkHttp.Server;

internal class HttpServerContext
{
    internal HttpRequest Request { get; }
    internal HttpResponse Response { get; }

    public HttpServerContext(HttpRequest request, HttpResponse response)
    {
        Request = request;
        Response = response;
    }
}
