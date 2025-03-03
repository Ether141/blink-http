using BlinkHttp.Http;
using BlinkHttp.Routing;
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

        public override void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            string path = request.Url!.PathAndQuery;

            IEndpoint? endpoint = router.GetEndpoint(path);

            if (endpoint == null)
            {
                buffer = ReturnNotFoundPage(response);
                return;
            }

            IHttpResult? result = endpoint.InvokeEndpoint();

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
