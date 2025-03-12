using BlinkHttp.Authentication;
using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal interface IEndpoint
    {
        Http.HttpMethod HttpMethod { get; }
        IEndpointMethod Method { get; }
        bool IsSecure { get; }
        AuthenticationRules? AuthenticationRules { get; }

        IHttpResult? InvokeEndpoint(HttpContext context, object?[]? args);
    }
}
