using BlinkHttp.Files;
using BlinkHttp.Http;
using BlinkHttp.Logging;
using System.Net;
using System.Text;

namespace BlinkHttp.Handling.Pipeline
{
    internal class StaticFiles : IMiddleware
    {
#pragma warning disable CS8618
        public MiddlewareDelegate Next { get; set; }
#pragma warning restore CS8618

        private readonly ILogger logger = LoggerFactory.Create<StaticFiles>();

        public async Task InvokeAsync(HttpContext context)
        {
            string localPath = FilesManager.GetLocalPathFile(context.Request.Url!);

            if (!FilesManager.FileExists(localPath))
            {
                await Next(context);
                return;
            }

            logger.Debug($"Serving requested static file: {localPath}");
            context.Buffer = FilesManager.LoadFile(context.Request.Url!);
            context.Response.ContentType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(localPath));
        }

        private static byte[] ReturnNotFoundPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageNotFound(), HttpStatusCode.NotFound);

        private static byte[] ReturnUnauthorizedPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageUnauthorizedError(), HttpStatusCode.Unauthorized);

        private static byte[] ReturnForbiddenPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageForbiddenError(), HttpStatusCode.Forbidden);

        private static byte[] ReturnPage(HttpListenerResponse response, string s, HttpStatusCode status)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(s);

            response.StatusCode = (int)status;
            response.ContentType = MimeTypes.TextHtml;

            return buffer;
        }
    }
}
