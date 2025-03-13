using BlinkHttp.Authentication;
using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Routing
{
    internal sealed class ControllerEndpoint : IEndpoint
    {
        public Http.HttpMethod HttpMethod { get; }
        public IEndpointMethod Method { get; }
        public bool IsSecure { get; }
        public AuthenticationRules? AuthenticationRules { get; }
        internal Controller Controller { get; }

        public ControllerEndpoint(Http.HttpMethod httpMethod, Controller controller, IEndpointMethod method)
        {
            HttpMethod = httpMethod;
            Controller = controller;
            Method = method;

            AuthorizeAttribute? attr = method.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() ?? Controller.GetType().GetCustomAttribute<AuthorizeAttribute>();
            IsSecure = attr != null;

            if (IsSecure)
            {
                AuthenticationRules = attr!.AuthenticationRules;
            }
        }

        public IHttpResult? InvokeEndpoint(object?[]? args)
        {
            return Method.Invoke(Controller, args) as IHttpResult;
        }

        public override string? ToString() => $"[{HttpMethod}] {Method}";
    }
}
