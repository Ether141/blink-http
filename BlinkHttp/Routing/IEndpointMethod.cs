using System.Reflection;

namespace BlinkHttp.Routing
{
    internal interface IEndpointMethod
    {
        MethodInfo MethodInfo { get; }
        bool MethodHasParameters { get; }
        object? Invoke(object? obj, object?[]? args);
    }
}
