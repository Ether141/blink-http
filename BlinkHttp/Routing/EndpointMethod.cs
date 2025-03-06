using System.Reflection;

namespace BlinkHttp.Routing
{
    internal class EndpointMethod : IEndpointMethod
    {
        public MethodInfo MethodInfo { get; }
        public bool MethodHasParameters => MethodInfo.GetParameters().Length > 0;

        internal EndpointMethod(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }

        public object? Invoke(object? obj, object?[]? args) => MethodInfo.Invoke(obj, args);
    }
}
