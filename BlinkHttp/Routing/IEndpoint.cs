using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal interface IEndpoint
    {
        Http.HttpMethod HttpMethod { get; }
        IEndpointMethod Method { get; }

        IHttpResult? InvokeEndpoint(HttpContext context, object?[]? args);
    }
}
