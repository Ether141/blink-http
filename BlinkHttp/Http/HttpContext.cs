using System.Net;

namespace BlinkHttp.Http;

internal class HttpContext
{
    internal HttpListenerRequest Request { get; }
    internal HttpListenerResponse Response { get; }

    public HttpContext(HttpListenerRequest request, HttpListenerResponse response)
    {
        Request = request;
        Response = response;
    }
}
