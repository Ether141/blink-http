using System.Net;

namespace BlinkHttp.Authentication.Session;

internal static class CookieHelper
{
    internal const string SessionIdCookieName = "session_id";

    internal static void SetSessionCookie(HttpListenerResponse response, SessionInfo sessionInfo) 
        => response.Cookies.Add(CreateCookie(SessionIdCookieName, sessionInfo.SessionId));

    internal static string? GetSessionIdFromCookie(HttpListenerRequest request) => request.Cookies[SessionIdCookieName]?.Value;

    private static Cookie CreateCookie(string name, string value) => new Cookie(name, value)
    {
        HttpOnly = true,
        Secure = false,
        Path = "/",
        Domain = "localhost"
    };
}
