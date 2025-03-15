using BlinkDatabase.General;
using BlinkHttp.Authentication;
using System.Net;

namespace BlinkHttp.Http;

internal class ControllerContext
{
    internal HttpListenerRequest? Request { get; set; }
    internal HttpListenerResponse? Response { get; set; }
    internal IUser? User { get; set; }

    internal ControllerContext(HttpListenerRequest? request, HttpListenerResponse? response)
    {
        Request = request;
        Response = response;
    }
}
