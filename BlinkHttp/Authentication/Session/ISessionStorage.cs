namespace BlinkHttp.Authentication.Session;

internal interface ISessionStorage
{
    void AddSession(SessionInfo sessionInfo);
    void RemoveSession(string sessionInfo);
    void RemoveAllSesions(int userId);
    SessionInfo? GetSessionInfoById(string sessionId);
}
