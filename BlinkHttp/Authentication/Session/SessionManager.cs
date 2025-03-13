using System.Net;

namespace BlinkHttp.Authentication.Session;

/// <summary>
/// <seealso cref="IAuthorizer"/> that provides functionalities to handle session based authorization and authentication.
/// </summary>
public sealed class SessionManager : IAuthorizer
{
    private readonly ISessionStorage sessionStorage;
    private readonly IAuthenticationProvider authenticationProvider;
    private readonly IUserInfoProvider userInfoProvider;

    private readonly Dictionary<string, LoginAttempt> loggingAttempts = [];

    private bool attemptsLimitingEnabled = false;
    private int attemptCooldown;
    private int attemptLimit;

    private TimeSpan? sessionValidFor;

    internal SessionManager(ISessionStorage sessionStorage, IAuthenticationProvider authenticationProvider, IUserInfoProvider userInfoProvider)
    {
        this.sessionStorage = sessionStorage;
        this.authenticationProvider = authenticationProvider;
        this.userInfoProvider = userInfoProvider;
    }

    internal void EnableAttemptsLimiting(int cooldown, int limit)
    {
        attemptsLimitingEnabled = true;
        attemptCooldown = cooldown;
        attemptLimit = limit;
    }

    internal void EnableSessionExpiration(TimeSpan duration) => sessionValidFor = duration;

    /// <summary>
    /// Attempts to login user with given username and password. If credentials are valid, creates new session for this user, tracks it and optionally adds session cookie to the given <seealso cref="HttpListenerResponse"/>.
    /// </summary>
    /// <param name="ipAddress">IP address of the user which will be used to track number of failed login attempts.</param>
    /// <param name="response">Response which session cookie will be assigned to, when logging operation is successful.</param>
    public (CredentialsValidationResult, SessionInfo?, IUser?) Login(string username, string password, string ipAddress, HttpListenerResponse? response)
    {
        CredentialsValidationResult result = authenticationProvider.ValidateCredentials(username, password, out IUser? user);

        if (result != CredentialsValidationResult.Success)
        {
            if (attemptsLimitingEnabled)
            {
                if (!loggingAttempts.TryGetValue(ipAddress, out LoginAttempt? loginAttempt))
                {
                    loginAttempt = new LoginAttempt(attemptCooldown, attemptLimit);
                    loggingAttempts[ipAddress] = loginAttempt;
                }

                if (!loginAttempt!.RegisterAttempt(DateTimeOffset.Now.ToUnixTimeSeconds()))
                {
                    return (CredentialsValidationResult.TooManyRequests, null, null);
                }
            }

            return (result, null, null);
        }

        SessionInfo createdSession = CreateNewSession(user!);
        
        if (response != null)
        {
            CookieHelper.SetSessionCookie(response, createdSession);
        }

        if (attemptsLimitingEnabled)
        {
            if (loggingAttempts.TryGetValue(ipAddress, out LoginAttempt? loginAttempt))
            {
                loggingAttempts[ipAddress].ResetAttempts();
            }
        }

        return (CredentialsValidationResult.Success, createdSession, user!);
    }

    /// <summary>
    /// Invalids session from session cookie from given <seealso cref="HttpListenerRequest"/> and deletes it from the session storage.
    /// </summary>
    /// <param name="request"></param>
    public void Logout(HttpListenerRequest request)
    {
        string? sessionId = CookieHelper.GetSessionIdFromCookie(request);

        if (sessionId == null)
        {
            return;
        }

        sessionStorage.RemoveSession(sessionId);
    }

    /// <summary>
    /// Attempts to authorize given <seealso cref="HttpListenerRequest"/> using session ID from cookie, if it's present in the request. Also optionally checks user priviliges, based on given <seealso cref="AuthenticationRules"/> if provided.
    /// </summary>
    public AuthorizationResult Authorize(HttpListenerRequest request, AuthenticationRules? rules)
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
    public void InvalidAllSessions(int userId) => sessionStorage.RemoveAllSesions(userId);

    private bool DidSessionExpire(SessionInfo sessionInfo) => sessionValidFor != null && DateTime.Now > sessionInfo.CreatedAt + sessionValidFor.Value;

    private SessionInfo CreateNewSession(IUser user)
    {
        string sessionId = SessionIdProvider.GenerateSessionId();
        SessionInfo sessionInfo = new SessionInfo(sessionId, user.Id);
        sessionStorage.AddSession(sessionInfo);
        return sessionInfo;
    }
}
