﻿using BlinkHttp.Routing;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that endpoint is intended to be used only with GET method.
/// </summary>
/// <remarks>Optionally routing value can be set. Default routing will be name of the method in lowercase.</remarks>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class HttpGetAttribute : HttpAttribute
{
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public HttpGetAttribute() { }

    public HttpGetAttribute(string routing) : base(routing) { }
}
