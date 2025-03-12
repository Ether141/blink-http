using System.Net;

namespace BlinkHttp.Authentication;

public class AuthorizationResult
{
    public bool Authorized { get; }
    public HttpStatusCode HttpCode { get; }
    public string Message { get; }

    internal const string SuccessMessage = "Success";
    internal const string UnauthorizedMessage = "Unauthorized";
    internal const string ForbiddenMessage = "Forbidden";
    internal const string SessionExpiredMessage = "Session expired";

    public AuthorizationResult(bool authorized, HttpStatusCode httpCode, string message)
    {
        Authorized = authorized;
        HttpCode = httpCode;
        Message = message;
    }
}
