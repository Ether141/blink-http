using BlinkHttp.Files;
using System.Net;

namespace BlinkHttp.Handling
{
    internal class GeneralRequestHandler : IRequestHandler
    {
        public void HandleRequest(HttpListenerRequest request, HttpListenerResponse response, ref byte[] buffer)
        {
            RequestType requestType = DetermineRequestType(request);
            IRequestHandler handler = GetRequestHandler(requestType);
            handler.HandleRequest(request, response, ref buffer);
        }

        private static RequestType DetermineRequestType(HttpListenerRequest request)
        {
            if (request.Url!.AbsolutePath == "/" || FilesManager.FileExists(request.Url!))
            {
                return RequestType.File;
            }
            else if (false)
            {
                return RequestType.Rest;
            }
            else
            {
                return RequestType.None;
            }
        }

        private static IRequestHandler GetRequestHandler(RequestType type) =>
            type switch
            {
                RequestType.File => new StaticFilesRequestHandler(),
                RequestType.Rest => new RestRequestHandler(),
                RequestType.None => new NotFoundRequestHandler(),
                _ => throw new InvalidOperationException()
            };
    }
}
