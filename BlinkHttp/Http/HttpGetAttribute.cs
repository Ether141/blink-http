using BlinkHttp.Routing;

namespace BlinkHttp.Http;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpGetAttribute : HttpAttribute
{
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public HttpGetAttribute() { }

    public HttpGetAttribute(string routing) : base(routing) { }
}
