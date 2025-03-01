using System.Net;

namespace BlinkHttp.Handling
{
    internal interface IRequestHandler
    {
        void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer);
    }
}
