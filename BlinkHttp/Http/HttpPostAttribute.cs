using BlinkHttp.Routing;

namespace BlinkHttp.Http;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpPostAttribute : HttpAttribute
{
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public HttpPostAttribute() { }

    public HttpPostAttribute(string routing) : base(routing) { }
}
