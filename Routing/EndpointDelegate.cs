using BlinkHttp.Http;

namespace BlinkHttp.Routing
{
    internal class EndpointDelegate : IEndpointMethod
    {
        private readonly Func<IHttpResult> func;

        public EndpointDelegate(Func<IHttpResult> func)
        {
            this.func = func;
        }

        public object? Invoke(object? obj, object?[]? args) => func.Invoke();
    }
}
