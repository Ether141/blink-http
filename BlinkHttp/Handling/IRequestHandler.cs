using BlinkHttp.Http;

namespace BlinkHttp.Handling;

internal interface IRequestHandler
{
    void HandleRequest(ControllerContext context, ref byte[] buffer);
}
