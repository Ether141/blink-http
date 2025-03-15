using BlinkHttp.Authentication;
using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Basic class for all server controllers. It contains endpoints, that can be used in HTTP requests.
/// </summary>
public abstract class Controller
{
#pragma warning disable CS8618
    /// <summary>
    /// Currently handled request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerRequest? Request { get; internal set; }

    /// <summary>
    /// Response that will be sent to the user, after complete handling of request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerResponse? Response { get; internal set; }

    /// <summary>
    /// Information about user that is associated with current request. Available only from endpoint method, marked with <seealso cref="AuthorizeAttribute"/> and when authorization was turned on during building <seealso cref="BlinkHttp.Application.WebApplication"/>.
    /// </summary>
    public IUser? User { get; internal set; }
#pragma warning restore CS8618
}
