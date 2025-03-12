using BlinkHttp.Authentication;
using System.Net;

namespace BlinkHttp.Http;

public class HttpContext
{
    public HttpListenerRequest Request { get; }
    public HttpListenerResponse Response { get; }
    public IAuthorizer? Authorizer { get; }

    public HttpContext(HttpListenerRequest request, HttpListenerResponse response, IAuthorizer? authorizer)
    {
        Request = request;
        Response = response;
        Authorizer = authorizer;
    }
}
