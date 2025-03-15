namespace BlinkHttp.Authentication.Session;

internal class SessionStorageInMemory : ISessionStorage
{
    private readonly Dictionary<string, List<string>> userSessions = [];
    private readonly Dictionary<string, SessionInfo> sessions = [];

    public void AddSession(SessionInfo sessionInfo)
    {
        if (sessions.ContainsKey(sessionInfo.SessionId))
        {
            throw new SessionIdAlreadyExistsException();
        }

        sessions[sessionInfo.SessionId] = sessionInfo;

        if (!userSessions.ContainsKey(sessionInfo.UserId))
        {
            userSessions[sessionInfo.UserId] = []; 
        }

        userSessions[sessionInfo.UserId].Add(sessionInfo.SessionId);
    }

    public void RemoveSession(string sessionId)
    {
        if (!sessions.ContainsKey(sessionId))
        {
            return;
        }

        SessionInfo sessionInfo = GetSessionInfoById(sessionId)!;
        sessions.Remove(sessionId);
        userSessions[sessionInfo.UserId].Remove(sessionId);

        if (userSessions[sessionInfo.UserId].Count == 0)
        {
            userSessions.Remove(sessionInfo.UserId);
        }
    }

    public void RemoveAllSesions(string userId)
    {
        if (!userSessions.TryGetValue(userId, out List<string>? allSessionsForUser))
        {
            return;
        }

        foreach (string sessionId in allSessionsForUser)
        {
            sessions.Remove(sessionId);
        }

        userSessions.Remove(userId);
    }

    public SessionInfo? GetSessionInfoById(string sessionId) => sessions.TryGetValue(sessionId, out SessionInfo? sessionInfo) ? sessionInfo : null;
}
