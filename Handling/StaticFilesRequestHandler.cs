using BlinkHttp.Files;
using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class StaticFilesRequestHandler : RequestHandler
    {
        public override void HandleRequest(HttpContext context, ref byte[] buffer)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string localPath = FilesManager.GetLocalPathFile(request.Url!);
            Console.WriteLine($"Returning static file: {localPath}");

            try
            {
                buffer = FilesManager.LoadFile(request.Url!);
            }
            catch (FileNotFoundException)
            {
                buffer = ReturnNotFoundPage(response);
                return;
            }

            response.ContentType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(localPath));
            response.ContentLength64 = buffer.Length;
        }
    }
}
