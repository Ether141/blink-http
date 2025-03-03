using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal class SingleEndpoint : IEndpoint
    {
        public Http.HttpMethod HttpMethod { get; }
        public IEndpointMethod Method { get; }

        public SingleEndpoint(Http.HttpMethod httpMethod, IEndpointMethod method)
        {
            HttpMethod = httpMethod;
            Method = method;
        }

        public IHttpResult? InvokeEndpoint() => Method.Invoke(null, null) as IHttpResult;
    }
}
