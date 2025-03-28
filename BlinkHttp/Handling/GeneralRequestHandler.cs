using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.Files;
using BlinkHttp.Http;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Handling;

internal class GeneralRequestHandler : RequestHandler
{
    private readonly Router router;
    private readonly IAuthorizer? authorizer;
    private readonly IConfiguration? configuration;

    public GeneralRequestHandler(Router router, IAuthorizer? authorizer, IConfiguration? configuration)
    {
        this.router = router;
        this.authorizer = authorizer;
        this.configuration = configuration;
    }

    public override void HandleRequest(ControllerContext context, ref byte[] buffer)
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
            RequestType.File => new StaticFilesRequestHandler(configuration, authorizer),
            RequestType.Rest => new RestRequestHandler(router, authorizer),
            _ => throw new NotSupportedException("RequestType is undefined.")
        };
}
