using BlinkHttp.Files;
using BlinkHttp.Http;
using Logging;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class StaticFilesRequestHandler : RequestHandler
    {
        private readonly ILogger logger = Logger.GetLogger(typeof(StaticFilesRequestHandler));

        public override void HandleRequest(ControllerContext context, ref byte[] buffer)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string localPath = FilesManager.GetLocalPathFile(request.Url!);
            logger.Debug($"Requested static file: {localPath}");

            try
            {
                buffer = FilesManager.LoadFile(request.Url!);
            }
            catch (FileNotFoundException)
            {
                buffer = ReturnNotFoundPage(response);
                logger.Debug("File cannot be found on the server.");
                return;
            }

            response.ContentType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(localPath));
            response.ContentLength64 = buffer.Length;
        }
    }
}
