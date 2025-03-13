using BlinkDatabase.General;
using BlinkHttp.Authentication;
using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Context for controller. Contains references to currently handled request and response, to database connection etc.
/// </summary>
public class HttpContext
{
    /// <summary>
    /// Currently handled request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerRequest? Request { get; internal set; }

    /// <summary>
    /// Response that will be sent to the user, after complete handling of request. When accessed from endpoint method, is never null.
    /// </summary>
    public HttpListenerResponse? Response { get; internal set; }

    /// <summary>
    /// Current authorization and authentication provider. It has references only if authorization was turned on during building <seealso cref="BlinkHttp.Application.WebApplication"/>.
    /// </summary>
    public IAuthorizer? Authorizer { get; internal set; }

    /// <summary>
    /// Current database connection handler. It can be used to create repositories. It has references only if database support was turned on during building <seealso cref="BlinkHttp.Application.WebApplication"/>.
    /// </summary>
    public IDatabaseConnection? DatabaseConnection { get; internal set; }

    /// <summary>
    /// Information about user that is associated with current request. Available only from endpoint method, marked with <seealso cref="AuthorizeAttribute"/> and when authorization was turned on during building <seealso cref="BlinkHttp.Application.WebApplication"/>.
    /// </summary>
    public IUser? User { get; internal set; }

    internal HttpContext(HttpListenerRequest? request, HttpListenerResponse? response, IAuthorizer? authorizer, IDatabaseConnection? databaseConnection)
    {
        Request = request;
        Response = response;
        Authorizer = authorizer;
        DatabaseConnection = databaseConnection;
    }
}
