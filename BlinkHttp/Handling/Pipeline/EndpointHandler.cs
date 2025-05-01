using BlinkHttp.Http;
using BlinkHttp.Serialization;
using Logging;
using System.Net;

namespace BlinkHttp.Handling.Pipeline;

internal class EndpointHandler : IMiddleware
{
    public MiddlewareDelegate Next { get; set; }

    private readonly ILogger logger = Logger.GetLogger<EndpointHandler>();

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request!.Url!.PathAndQuery;
        object?[]? args;

        try
        {
            args = RequestDataHandler.GetArguments(context.Route!, path, context.Request);
        }
        catch
        {
            logger.Debug("Request data is bad.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        Controller? controller = ControllersFactory.Factory.CreateController(context.Route!.AssociatedRoute.ControllerType, context);

        if (controller == null)
        {
            logger.Error($"Unable to instantiate controller '{context.Route!.AssociatedRoute.ControllerType}'. Check whether all dependenciec can be resolved!");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        object? resultObject = null;

        if (context.Route.Endpoint.IsAwaitable)
        {
            object? invokeResult = context.Route.Endpoint.InvokeEndpoint(controller, args);

            if (invokeResult is Task<IHttpResult> task)
            {
                resultObject = await task;
            }
            else
            {
                logger.Error("Internal server error.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return;
            }
        }
        else
        {
            resultObject = context.Route.Endpoint.InvokeEndpoint(controller, args);
        }

        if (resultObject == null)
        {
            logger.Error("Internal server error.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        IHttpResult result = (IHttpResult)resultObject;
        context.Buffer = result.Data;
        context.Response.ContentType = result.ContentType;
        context.Response.StatusCode = (int)result.HttpCode;

        if (result.ContentDisposition != null)
        {
            context.Response.AddHeader(HttpTypicalHeaders.ContentDisposition, result.ContentDisposition);
        }

        logger.Debug($"ContentLength: {context.Buffer.Length} | ContentType: {context.Response.ContentType} | ContentDisposition: {result.ContentDisposition ?? "N/A"}");

        await Task.CompletedTask;
    }
}
