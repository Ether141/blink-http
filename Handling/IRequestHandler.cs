using BlinkHttp.Http;

namespace BlinkHttp.Handling;

internal interface IRequestHandler
{
    void HandleRequest(HttpContext context, ref byte[] buffer);
}
