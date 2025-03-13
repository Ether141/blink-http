using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with DELETE method.
/// </summary>
/// <remarks>Optionally routing value can be set. Default routing will be name of the method in lowercase.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpDeleteAttribute : HttpAttribute
{
    public override HttpMethod HttpMethod => HttpMethod.Delete;

    public HttpDeleteAttribute() { }

    public HttpDeleteAttribute(string routing) : base(routing) { }
}
