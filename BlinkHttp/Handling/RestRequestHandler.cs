using BlinkHttp.Http;
using Logging;
using BlinkHttp.Routing;
using BlinkHttp.Serialization;
using System.Net;
using BlinkHttp.Authentication;
using System.Text;

namespace BlinkHttp.Handling
{
    internal class RestRequestHandler : RequestHandler
    {
        private readonly Router router;
        private readonly IAuthorizer? authorizer;

        private readonly ILogger logger = Logger.GetLogger<RestRequestHandler>();

        internal RestRequestHandler(Router router, IAuthorizer? authorizer)
        {
            this.router = router;
            this.authorizer = authorizer;
        }

        public override void HandleRequest(ControllerContext context, ref byte[] buffer)
        {
            HttpListenerResponse response = context.Response!;
            string path = context.Request!.Url!.PathAndQuery;
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

            IEndpoint endpoint = route.Endpoint;

            if (endpoint.IsSecure)
            {
                if (authorizer == null)
                {
                    logger.Warning("Endpoint is secure (marked with [Authorize] attribute), but authorization is turned off on server.");
                }
                else
                {
                    AuthorizationResult authorizationResult = authorizer.Authorize(context.Request, endpoint.AuthenticationRules);

                    if (!authorizationResult.Authorized)
                    {
                        logger.Debug($"This endpoint is secure and requires authorization, but client did not provide required credentials. Reason: {authorizationResult.Message}");
                        response.StatusCode = (int)authorizationResult.HttpCode;
                        buffer = Encoding.UTF8.GetBytes(authorizationResult.Message);
                        response.ContentLength64 = buffer.Length;
                        return; 
                    }

                    context.User = authorizationResult.User;
                }
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

            Controller controller = ControllersFactory.Factory.CreateController(route.AssociatedRoute.ControllerType, context);
            IHttpResult? result = endpoint.InvokeEndpoint(controller, args);

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
            response.StatusCode = (int)result.HttpCode;

            if (result.ContentDisposition != null)
            {
                response.AddHeader(HttpTypicalHeaders.ContentDisposition, result.ContentDisposition);
            }

            logger.Debug($"ContentLength: {buffer.Length} | ContentType: {response.ContentType} | ContentDisposition: {result.ContentDisposition ?? "N/A"}");
        }
    }
}
