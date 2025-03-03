using System.Reflection;

namespace BlinkHttp.Routing
{
    internal class EndpointMethod : IEndpointMethod
    {
        private readonly MethodInfo methodInfo;

        public EndpointMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        public object? Invoke(object? obj, object?[]? args) => methodInfo.Invoke(obj, args);
    }
}
