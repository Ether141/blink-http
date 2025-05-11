using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Server.Default;

internal class SimpleServerResponse : HttpResponse
{
    public override Stream OutputStream => wrapped.OutputStream;

    public override int StatusCode
    {
        get => wrapped.StatusCode;
        set => wrapped.StatusCode = value;
    }

    public override long ContentLength64
    {
        get => wrapped.ContentLength64;
        set => wrapped.ContentLength64 = value;
    }

    public override string? ContentType
    {
        get => wrapped.ContentType;
        set => wrapped.ContentType = value;
    }

    public override CookieCollection Cookies => wrapped.Cookies;

    public override Version ProtocolVersion
    {
        get => wrapped.ProtocolVersion;
        set => wrapped.ProtocolVersion = value;
    }

    public override bool KeepAlive
    {
        get => wrapped.KeepAlive;
        set => wrapped.KeepAlive = value;
    }

    public override WebHeaderCollection Headers
    {
        get => wrapped.Headers;
        set => wrapped.Headers = value;
    }

    public override bool SendChunked
    {
        get => wrapped.SendChunked;
        set => wrapped.SendChunked = value;
    }

    private readonly HttpListenerResponse wrapped;

    internal SimpleServerResponse(HttpListenerResponse wrapped)
    {
        this.wrapped = wrapped;
    }

    public override void Close() => wrapped.Close();

    public override void AddHeader(string name, string value) => wrapped.AddHeader(name, value);

    public override void Redirect(string url) => wrapped.Redirect(url);

    public override void AppendHeader(string name, string value) => wrapped.AppendHeader(name, value);

    public override void AppendCookie(Cookie cookie) => wrapped.AppendCookie(cookie);
}
