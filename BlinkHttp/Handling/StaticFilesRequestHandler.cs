//using BlinkHttp.Authentication;
//using BlinkHttp.Configuration;
//using BlinkHttp.Files;
//using BlinkHttp.Http;
//using Logging;
//using System.Net;
//using System.Text;

//namespace BlinkHttp.Handling
//{
//    internal class StaticFilesRequestHandler : RequestHandler
//    {
//        private readonly ILogger logger = Logger.GetLogger<StaticFilesRequestHandler>();
//        private readonly IConfiguration? configuration;
//        private readonly IAuthorizer? authorizer;

//        internal StaticFilesRequestHandler(IConfiguration? configuration, IAuthorizer? authorizer)
//        {
//            this.configuration = configuration;
//            this.authorizer = authorizer;
//        }

//        public override void HandleRequest(ControllerContext context, ref byte[] buffer)
//        {
//            HttpListenerRequest request = context.Request;
//            HttpListenerResponse response = context.Response;

//            string localPath = FilesManager.GetLocalPathFile(request.Url!);
//            logger.Debug($"Requested static file: {localPath}");

//            try
//            {
//                buffer = FilesManager.LoadFile(request.Url!);
//            }
//            catch (FileNotFoundException)
//            {
//                buffer = ReturnNotFoundPage(response);
//                logger.Debug("File cannot be found on the server.");
//                return;
//            }

//            AuthorizationResult? authorizationResult = HasAccess(request);

//            if (authorizationResult != null && !authorizationResult.Authorized)
//            {
//                if (authorizationResult.HttpCode == HttpStatusCode.Forbidden)
//                {
//                    buffer = ReturnForbiddenPage(response);
//                }
//                else
//                {
//                    buffer = ReturnUnauthorizedPage(response);
//                }

//                return;
//            }

//            response.ContentType = MimeTypes.GetMimeTypeForExtension(Path.GetExtension(localPath));
//            response.ContentLength64 = buffer.Length;
//        }

//        private AuthorizationResult? HasAccess(HttpListenerRequest request)
//        {
//            if (configuration == null || authorizer == null)
//            {
//                return null;
//            }

//            string fileName = request.Url!.AbsolutePath[1..];
//            string[] restrictions;

//            try
//            {
//                restrictions = configuration.GetArray("server:access_restriction");
//            }
//            catch (ApplicationConfigurationException)
//            {
//                return null;
//            }

//            string? accessRestriction = restrictions.FirstOrDefault(r => r.Split(':')[0] == fileName)?.Split(':')[1];

//            if (accessRestriction == null)
//            {
//                return null;
//            }

//            return authorizer.Authorize(request, new AuthenticationRules(null, [ accessRestriction ]));
//        }

//        private static byte[] ReturnNotFoundPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageNotFound(), HttpStatusCode.NotFound);

//        private static byte[] ReturnUnauthorizedPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageUnauthorizedError(), HttpStatusCode.Unauthorized);

//        private static byte[] ReturnForbiddenPage(HttpListenerResponse response) => ReturnPage(response, StaticHtmlResources.GetErrorPageForbiddenError(), HttpStatusCode.Forbidden);

//        private static byte[] ReturnPage(HttpListenerResponse response, string s, HttpStatusCode status)
//        {
//            byte[] buffer = Encoding.UTF8.GetBytes(s);

//            response.StatusCode = (int)status;
//            response.ContentType = MimeTypes.TextHtml;
//            response.ContentLength64 = buffer.Length;

//            return buffer;
//        }
//    }
//}
