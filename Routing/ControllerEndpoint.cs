using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal sealed class ControllerEndpoint : IEndpoint
    {
        public Http.HttpMethod HttpMethod { get; }
        public IEndpointMethod Method { get; }
        internal Controller Controller { get; }

        public ControllerEndpoint(Http.HttpMethod httpMethod, Controller controller, IEndpointMethod method)
        {
            HttpMethod = httpMethod;
            Controller = controller;
            Method = method;
        }

        public IHttpResult? InvokeEndpoint() => Method.Invoke(Controller, null) as IHttpResult;

        public override string? ToString() => $"[{HttpMethod}] {Method}";
    }
}
