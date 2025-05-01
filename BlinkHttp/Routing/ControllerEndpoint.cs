using BlinkHttp.Authentication;
using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Routing;

internal sealed class ControllerEndpoint : IEndpoint
{
    public Http.HttpMethod HttpMethod { get; }
    public MethodInfo MethodInfo { get; }
    public bool MethodHasParameters { get; }
    public bool IsSecure { get; }
    public bool IsAwaitable { get; }
    public AuthenticationRules? AuthenticationRules { get; }
    internal Type ControllerType { get; }

    public ControllerEndpoint(Http.HttpMethod httpMethod, Type controllerType, MethodInfo methodInfo)
    {
        HttpMethod = httpMethod;
        ControllerType = controllerType;
        MethodInfo = methodInfo;

        AuthorizeAttribute? attr = MethodInfo.GetCustomAttribute<AuthorizeAttribute>() ?? controllerType.GetCustomAttribute<AuthorizeAttribute>();

        IsAwaitable = MethodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
        IsSecure = MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() == null && attr != null;
        MethodHasParameters = MethodInfo.GetParameters().Length > 0;

        if (IsSecure)
        {
            AuthenticationRules = attr!.AuthenticationRules;
        }
    }

    public object? InvokeEndpoint(Controller controller, object?[]? args) => 
        IsAwaitable ? MethodInfo.Invoke(controller, args) as Task<IHttpResult> : MethodInfo.Invoke(controller, args) as IHttpResult;

    public override string? ToString() => $"[{HttpMethod}] {MethodInfo.Name}";
}
