using BlinkHttp.Http;
using System.Net;
using System.Text;

namespace BlinkHttp.Handling
{
    internal class RestRequestHandler : IRequestHandler
    {
        public void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            string responseString = "Witaj! To jest prosty serwer HTTP w C#.";
            buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentType = $"{MimeTypes.TextPlain}; charset=utf-8";
        }
    }
}
