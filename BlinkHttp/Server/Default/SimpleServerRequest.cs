using BlinkHttp.Http;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace BlinkHttp.Server.Default;

internal class SimpleServerRequest : HttpRequest
{
    public override string HttpMethod => wrapped.HttpMethod;

    public override IPEndPoint LocalEndPoint => wrapped.LocalEndPoint;

    public override Uri? Url => wrapped.Url;

    public override NameValueCollection Headers => wrapped.Headers;

    public override CookieCollection Cookies => wrapped.Cookies;

    public override IPEndPoint RemoteEndPoint => wrapped.RemoteEndPoint;

    public override long ContentLength64 => wrapped.ContentLength64;

    public override string? ContentType => wrapped.ContentType;

    public override Encoding ContentEncoding => wrapped.ContentEncoding;

    public override bool HasEntityBody => wrapped.HasEntityBody;

    public override Stream InputStream => wrapped.InputStream;

    public override Version ProtocolVersion => wrapped.ProtocolVersion;

    public override bool IsAuthenticated => wrapped.IsAuthenticated;

    public override bool IsLocal => wrapped.IsLocal;

    public override bool IsSecureConnection => wrapped.IsSecureConnection;

    public override string? ServiceName => wrapped.ServiceName;

    public override string? UserAgent => wrapped.UserAgent;

    public override string? UserHostAddress => wrapped.UserHostAddress;

    public override string? UserHostName => wrapped.UserHostName;

    public override string[]? UserLanguages => wrapped.UserLanguages;

    private readonly HttpListenerRequest wrapped;

    internal SimpleServerRequest(HttpListenerRequest wrapped)
    {
        this.wrapped = wrapped;
    }
}
