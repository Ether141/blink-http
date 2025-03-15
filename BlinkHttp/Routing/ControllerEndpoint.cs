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
        internal Type ControllerType { get; }

        public ControllerEndpoint(Http.HttpMethod httpMethod, Type controllerType, IEndpointMethod method)
        {
            HttpMethod = httpMethod;
            ControllerType = controllerType;
            Method = method;

            AuthorizeAttribute? attr = method.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() ?? controllerType.GetCustomAttribute<AuthorizeAttribute>();
            IsSecure = attr != null;

            if (IsSecure)
            {
                AuthenticationRules = attr!.AuthenticationRules;
            }
        }

        public IHttpResult? InvokeEndpoint(Controller controller, object?[]? args) => Method.Invoke(controller, args) as IHttpResult;

        public override string? ToString() => $"[{HttpMethod}] {Method}";
    }
}
