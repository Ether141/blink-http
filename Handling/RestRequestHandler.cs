using BlinkHttp.Http;
using BlinkHttp.Routing;
using BlinkHttp.Serialization;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class RestRequestHandler : RequestHandler
    {
        private readonly Router router;

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
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ContentLength64 = 0;
                return;
            }

            IEndpoint endpoint = route.Endpoint!;
            IHttpResult? result = endpoint.InvokeEndpoint(context, args);

            if (result == null)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentLength64 = 0;
                return;
            }

            buffer = result.Data;
            response.ContentType = result.ContentType;
            response.ContentLength64 = buffer.Length;
        }
    }
}
