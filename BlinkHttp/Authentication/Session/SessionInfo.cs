namespace BlinkHttp.Authentication.Session;

public class SessionInfo
{
    public string SessionId { get; }
    public string UserId { get; }
    public DateTime CreatedAt { get; }

    public SessionInfo(string sessionId, string userId)
    {
        SessionId = sessionId;
        UserId = userId;
        CreatedAt = DateTime.Now;
    }
}
