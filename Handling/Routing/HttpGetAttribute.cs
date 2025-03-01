namespace BlinkHttp.Handling.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class HttpGetAttribute : Attribute
    {
        public string Routing { get; }

        public HttpGetAttribute(string routing)
        {
            Routing = routing;
        }
    }
}
