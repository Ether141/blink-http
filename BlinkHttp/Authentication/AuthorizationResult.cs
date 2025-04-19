using System.Net;

namespace BlinkHttp.Authentication;

/// <summary>
/// Encapsulates information about an authorization attempt.
/// </summary>
public class AuthorizationResult
{
    /// <summary>
    /// Indicates whether the authorization attemps was successful and user can access secured resource.
    /// </summary>
    public bool Authorized { get; }

    /// <summary>
    /// HTTP status code for response.
    /// </summary>
    public HttpStatusCode HttpCode { get; }

    /// <summary>
    /// Message for better understanding reason of failed authorization.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// User which was authorized successfully, otherwise null.
    /// </summary>
    public IUser? User { get; }

    internal const string SuccessMessage = "Success";
    internal const string UnauthorizedMessage = "Unauthorized";
    internal const string ForbiddenMessage = "Forbidden";
    internal const string SessionExpiredMessage = "Session expired";

    internal AuthorizationResult(bool authorized, HttpStatusCode httpCode, string message, IUser? user)
    {
        Authorized = authorized;
        HttpCode = httpCode;
        Message = message;
        User = user;
    }
}
