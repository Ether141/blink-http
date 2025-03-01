using BlinkHttp.Files;
using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class StaticFilesRequestHandler : IRequestHandler
    {
        public void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            string localPath = FilesManager.GetLocalPathFile(request.Url!);
            Console.WriteLine($"Returning static file: {localPath}");
            buffer = File.ReadAllBytes(localPath);
            response.ContentType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(localPath));
            response.ContentLength64 = buffer.Length;
        }
    }
}
