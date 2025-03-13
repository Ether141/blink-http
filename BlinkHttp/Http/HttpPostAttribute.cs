using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with POST method.
/// </summary>
/// <remarks>Optionally routing value can be set. Default routing will be name of the method in lowercase.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpPostAttribute : HttpAttribute
{
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public HttpPostAttribute() { }

    public HttpPostAttribute(string routing) : base(routing) { }
}
