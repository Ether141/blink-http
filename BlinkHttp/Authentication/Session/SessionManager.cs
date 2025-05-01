using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Authentication.Session;

/// <summary>
/// <seealso cref="IAuthorizer"/> that provides functionalities to handle session based authorization and authentication.
/// </summary>
public sealed class SessionManager : IAuthorizer
{
    /// <summary>
    /// Gets the <seealso cref="IUserInfoProvider"/> used by this instance of <seealso cref="SessionManager"/>.
    /// </summary>
    public IUserInfoProvider UserInfoProvider => userInfoProvider;

    private readonly ISessionStorage sessionStorage;
    private readonly IAuthenticationProvider authenticationProvider;
    private readonly IUserInfoProvider userInfoProvider;

    private TimeSpan? sessionValidFor;

    internal SessionManager(ISessionStorage sessionStorage, IAuthenticationProvider authenticationProvider, IUserInfoProvider userInfoProvider)
    {
        this.sessionStorage = sessionStorage;
        this.authenticationProvider = authenticationProvider;
        this.userInfoProvider = userInfoProvider;
    }

    internal void EnableSessionExpiration(TimeSpan duration) => sessionValidFor = duration;

    /// <summary>
    /// Creates new session for given user ID, tracks this newly created session and optionally adds session cookie to the given <seealso cref="HttpResponse"/>.
    /// </summary>
    public SessionInfo CreateSession(string userId, HttpResponse? response)
    {
        SessionInfo createdSession = CreateNewSession(userId);

        if (response != null)
        {
            CookieHelper.SetSessionCookie(response, createdSession);
        }

        return createdSession;
    }

    /// <summary>
    /// Invalids session from session cookie from given <seealso cref="HttpRequest"/> and deletes it from the session storage.
    /// </summary>
    public void InvalidSession(HttpRequest request)
    {
        string? sessionId = CookieHelper.GetSessionIdFromCookie(request);

        if (sessionId == null)
        {
            return;
        }

        sessionStorage.RemoveSession(sessionId);
    }

    /// <summary>
    /// Attempts to authorize given <seealso cref="HttpRequest"/> using session ID from cookie, if it's present in the request. Also optionally checks user priviliges, based on given <seealso cref="AuthenticationRules"/> if provided.
    /// </summary>
    public AuthorizationResult Authorize(HttpRequest request, AuthenticationRules? rules)
    {
        string? sessionId = CookieHelper.GetSessionIdFromCookie(request);

        if (sessionId == null)
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.UnauthorizedMessage, null);
        }

        SessionInfo? sessionInfo = sessionStorage.GetSessionInfoById(sessionId);

        if (sessionInfo == null)
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.UnauthorizedMessage, null);
        }

        if (DidSessionExpire(sessionInfo))
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.SessionExpiredMessage, null);
        }

        IUser user = userInfoProvider.GetUser(sessionInfo.UserId)!;

        if (rules == null)
        {
            return new AuthorizationResult(true, HttpStatusCode.Accepted, AuthorizationResult.SuccessMessage, user);
        }

        if ((rules.OnlySelectedUsers && !rules.SelectedUsers!.Any(u => u == user.Username)) || (rules.OnlySelectedRoles && !rules.SelectedRoles!.Intersect(user.Roles).Any()))
        {
            return new AuthorizationResult(false, HttpStatusCode.Forbidden, AuthorizationResult.ForbiddenMessage, user);
        }

        return new AuthorizationResult(true, HttpStatusCode.Accepted, AuthorizationResult.SuccessMessage, user);
    }

    /// <summary>
    /// Invalids all sessions on all devices currently associated with the user with given ID.
    /// </summary>
    public void InvalidAllSessions(string userId) => sessionStorage.RemoveAllSesions(userId);

    private bool DidSessionExpire(SessionInfo sessionInfo) => sessionValidFor != null && DateTime.Now > sessionInfo.CreatedAt + sessionValidFor.Value;

    private SessionInfo CreateNewSession(string userId)
    {
        string sessionId = SessionIdProvider.GenerateSessionId();
        SessionInfo sessionInfo = new SessionInfo(sessionId, userId);
        sessionStorage.AddSession(sessionInfo);
        return sessionInfo;
    }
}
