using BlinkHttp.Authentication;
using BlinkHttp.Files;
using BlinkHttp.Http;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Handling;

internal class GeneralRequestHandler : RequestHandler
{
    private readonly Router router;
    private readonly IAuthorizer? authorizer;

    public GeneralRequestHandler(Router router, IAuthorizer? authorizer)
    {
        this.router = router;
        this.authorizer = authorizer;
    }

    public override void HandleRequest(HttpContext context, ref byte[] buffer)
    {
        RequestType requestType = DetermineRequestType(context.Request);
        IRequestHandler handler = GetRequestHandler(requestType);
        handler.HandleRequest(context, ref buffer);
    }

    private static RequestType DetermineRequestType(HttpListenerRequest request) =>
        request.Url!.AbsolutePath == "/" || FilesManager.FileExists(request.Url!) ? RequestType.File : RequestType.Rest;

    private IRequestHandler GetRequestHandler(RequestType type) =>
        type switch
        {
            RequestType.File => new StaticFilesRequestHandler(),
            RequestType.Rest => new RestRequestHandler(router, authorizer),
            _ => throw new NotSupportedException("RequestType is undefined.")
        };
}
