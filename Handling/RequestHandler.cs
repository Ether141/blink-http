using BlinkHttp.Http;
using System.Net;
using System.Text;

namespace BlinkHttp.Handling
{
    internal abstract class RequestHandler : IRequestHandler
    {
        public abstract void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer);

        protected static byte[] ReturnNotFoundPage(HttpListenerResponse response)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(StaticHtmlResources.GetErrorPageNotFound());

            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.ContentType = MimeTypes.TextHtml;
            response.ContentLength64 = buffer.Length;

            return buffer;
        }
    }
}
