namespace BlinkHttp.Authentication.Session;

internal interface ISessionStorage
{
    void AddSession(SessionInfo sessionInfo);
    void RemoveSession(string sessionInfo);
    void RemoveAllSesions(string userId);
    SessionInfo? GetSessionInfoById(string sessionId);
}
