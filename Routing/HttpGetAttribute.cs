namespace BlinkHttp.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class HttpGetAttribute : HttpAttribute
    {
        internal override Http.HttpMethod HttpMethod => Http.HttpMethod.Get;

        public HttpGetAttribute() { }

        public HttpGetAttribute(string routing) : base(routing) { }
    }
}
