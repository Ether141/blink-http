using BlinkHttp.Files;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class GeneralRequestHandler : RequestHandler
    {
        private readonly Router router;

        public GeneralRequestHandler(Router router)
        {
            this.router = router;
        }

        public override void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            RequestType requestType = DetermineRequestType(request);
            IRequestHandler handler = GetRequestHandler(requestType);
            handler.HandleRequest(request, response, ref buffer);
        }

        private static RequestType DetermineRequestType(HttpListenerRequest request) =>
            request.Url!.AbsolutePath == "/" || FilesManager.FileExists(request.Url!) ? RequestType.File : RequestType.Rest;

        private IRequestHandler GetRequestHandler(RequestType type) =>
            type switch
            {
                RequestType.File => new StaticFilesRequestHandler(),
                RequestType.Rest => new RestRequestHandler(router),
                _ => throw new InvalidOperationException("RequestType is undefined.")
            };
    }
}
