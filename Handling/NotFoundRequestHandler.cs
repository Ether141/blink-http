using System.Net;

namespace BlinkHttp.Handling
{
    internal class NotFoundRequestHandler : IRequestHandler
    {
        public void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentLength64 = 0;
        }
    }
}
