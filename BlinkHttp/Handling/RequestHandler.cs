using BlinkHttp.Http;
using System.Net;
using System.Text;

namespace BlinkHttp.Handling;

internal abstract class RequestHandler : IRequestHandler
{
    public abstract void HandleRequest(ControllerContext context, ref byte[] buffer);
}
