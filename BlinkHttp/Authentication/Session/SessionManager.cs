using System.Net;

namespace BlinkHttp.Authentication.Session;

public class SessionManager : IAuthorizer
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

    public (CredentialsValidationResult, SessionInfo?) Login(string username, string password, string ipAddress, HttpListenerResponse? response)
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
                return (CredentialsValidationResult.TooManyRequests, null);
            } 
        }

        CredentialsValidationResult result = authenticationProvider.ValidateCredentials(username, password, out IUser? user);

        if (result != CredentialsValidationResult.Success)
        {
            return (result, null);
        }

        SessionInfo createdSession = CreateNewSession(user!);
        
        if (response != null)
        {
            CookieHelper.SetSessionCookie(response, createdSession);
        }

        return (CredentialsValidationResult.Success, createdSession);
    }

    public void Logout(HttpListenerRequest request)
    {
        string? sessionId = CookieHelper.GetSessionIdFromCookie(request);

        if (sessionId == null)
        {
            return;
        }

        sessionStorage.RemoveSession(sessionId);
    }

    public AuthorizationResult Authorize(HttpListenerRequest request, AuthenticationRules? rules)
    {
        string? sessionId = CookieHelper.GetSessionIdFromCookie(request);

        if (sessionId == null)
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.UnauthorizedMessage);
        }

        SessionInfo? sessionInfo = sessionStorage.GetSessionInfoById(sessionId);

        if (sessionInfo == null)
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.UnauthorizedMessage);
        }

        if (DidSessionExpire(sessionInfo))
        {
            return new AuthorizationResult(false, HttpStatusCode.Unauthorized, AuthorizationResult.SessionExpiredMessage);
        }

        if (rules == null)
        {
            return new AuthorizationResult(true, HttpStatusCode.Accepted, AuthorizationResult.SuccessMessage);
        }

        IUser user = userInfoProvider.GetUser(sessionInfo.UserId)!;

        if ((rules.OnlySelectedUsers && !rules.SelectedUsers!.Any(u => u == user.Username)) || (rules.OnlySelectedRoles && !rules.SelectedRoles!.Intersect(user.Roles).Any()))
        {
            return new AuthorizationResult(false, HttpStatusCode.Forbidden, AuthorizationResult.ForbiddenMessage);
        }

        return new AuthorizationResult(true, HttpStatusCode.Accepted, AuthorizationResult.SuccessMessage);
    }

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
