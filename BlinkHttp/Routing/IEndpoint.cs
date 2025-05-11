using BlinkHttp.Authentication;
using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Routing;

internal interface IEndpoint
{
    Http.HttpMethod HttpMethod { get; }
    MethodInfo MethodInfo { get; }
    bool MethodHasParameters { get; }
    bool IsSecure { get; }
    bool IsAwaitable { get; }
    AuthenticationRules? AuthenticationRules { get; }

    object? InvokeEndpoint(Controller controller, object?[]? args);
}
