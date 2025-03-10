using BlinkHttp.Http;
using Logging;
using BlinkHttp.Routing;
using BlinkHttp.Serialization;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class RestRequestHandler : RequestHandler
    {
        private readonly Router router;
        private readonly ILogger logger = Logger.GetLogger(typeof(RestRequestHandler));

        internal RestRequestHandler(Router router)
        {
            this.router = router;
        }

        public override void HandleRequest(HttpContext context, ref byte[] buffer)
        {
            HttpListenerResponse response = context.Response;
            string path = context.Request.Url!.PathAndQuery;
            Route? route = router.GetRoute(path);

            if (route == null)
            {
                buffer = ReturnNotFoundPage(response);
                return;
            }

            if (!route.HttpMethod.ToString().Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                logger.Debug($"Received request method is not allowed for this route. Request method: {route.HttpMethod} | Required method: {context.Request.HttpMethod}");
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                response.ContentLength64 = 0;
                return;
            }

            object?[]? args;

            try
            {
                args = RequestDataHandler.GetArguments(route, path, context.Request);
            }
            catch
            {
                logger.Debug("Request data is bad.");
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ContentLength64 = 0;
                return;
            }

            IEndpoint endpoint = route.Endpoint!;
            IHttpResult? result = endpoint.InvokeEndpoint(context, args);

            if (result == null)
            {
                logger.Debug("Internal server error.");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentLength64 = 0;
                return;
            }

            buffer = result.Data;
            response.ContentType = result.ContentType;
            response.ContentLength64 = buffer.Length;

            if (result.ContentDisposition != null)
            {
                response.AddHeader(HttpTypicalHeaders.ContentDisposition, result.ContentDisposition);
            }

            logger.Debug($"ContentLength: {buffer.Length} | ContentType: {response.ContentType} | ContentDisposition: {result.ContentDisposition ?? "N/A"}");
        }
    }
}
