using BlinkHttp.Authentication;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Http;

public sealed class HttpContext
{
    public HttpListenerRequest Request { get; }
    public HttpListenerResponse Response { get; }

    public byte[]? Buffer { get; set; }
    public IUser? User { get; set; }

    internal Route? Route { get; set; }

    internal HttpContext(HttpListenerRequest request, HttpListenerResponse response)
    {
        Request = request;
        Response = response;
    }
}
